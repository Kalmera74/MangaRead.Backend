#!/bin/bash
set -euo pipefail

# ===========================
# CONFIGURATION
# ===========================
API="./src/MangaRead.API"
INFRASTRUCTURE="./src/MangaRead.Infrastructure"
API_OUTPUT_DIR="/usr/local/bin/manga-api"
SERVICE_NAME="manga-api"
DLL_NAME="MangaRead.API.dll"
SERVICE_FILE="/etc/systemd/system/${SERVICE_NAME}.service"

# ===========================
# HELPERS
# ===========================
log() {
    echo -e "\033[1;32m[+] $1\033[0m"
}

warn() {
    echo -e "\033[1;33m[!] $1\033[0m"
}

error() {
    echo -e "\033[1;31m[✗] $1\033[0m" >&2
    exit 1
}

# ===========================
# TASKS
# ===========================
migration() {
    log "Running EF Core migrations..."
    dotnet ef database update \
        --project "$INFRASTRUCTURE" \
        --startup-project "$API" \
        -- --environment Production
}

clean() {
    log "Cleaning project..."
    dotnet clean
    rm -rf "$API_OUTPUT_DIR"
}

restore() {
    clean
    log "Restoring dependencies..."
    dotnet restore
}

build() {
    restore
    migration
    log "Building API project..."
    dotnet build "$API"
}

publish() {
    build
    log "Publishing API to $API_OUTPUT_DIR..."
    export ASPNETCORE_ENVIRONMENT="Production"
    mkdir -p "$API_OUTPUT_DIR"
    dotnet publish "$API" --configuration Release --output "$API_OUTPUT_DIR"
}

create_service() {
    if [[ -f "$SERVICE_FILE" ]]; then
        warn "Service file already exists: $SERVICE_FILE"
        return
    fi

    log "Creating systemd service: $SERVICE_FILE"

    cat <<EOF > "$SERVICE_FILE"
[Unit]
Description=MangaLuck API

[Service]
WorkingDirectory=$API_OUTPUT_DIR
ExecStart=dotnet $API_OUTPUT_DIR/$DLL_NAME
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=$SERVICE_NAME
User=root

[Install]
WantedBy=multi-user.target
EOF
}

reload_service() {
    log "Reloading systemd and enabling service..."
    systemctl daemon-reload
    systemctl enable "$SERVICE_NAME"
    systemctl stop "$SERVICE_NAME" || true
    systemctl start "$SERVICE_NAME"
}

create_aliases() {
    log "Checking and adding aliases..."
    aliases=(
        "alias api-stat='systemctl status $SERVICE_NAME'"
        "alias api-start='systemctl start $SERVICE_NAME'"
        "alias api-stop='systemctl stop $SERVICE_NAME'"
        "alias api-restart='systemctl restart $SERVICE_NAME'"
    )

    for alias in "${aliases[@]}"; do
        if ! grep -qF "$alias" ~/.bashrc; then
            echo "$alias" >> ~/.bashrc
            log "Added: $alias"
        else
            warn "Alias already exists: $alias"
        fi
    done

    # Reload aliases in current shell
    source ~/.bashrc
}

# ===========================
# MAIN
# ===========================
publish
create_service
reload_service
create_aliases

log "API service created, started, and aliases added."
