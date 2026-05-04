#!/bin/bash

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BACKEND_DIR="MtgInventoryManagementApi/MtgInventoryManagement.Api"

echo -e "The backend is starting"
cd "$ROOT_DIR/$BACKEND_DIR"
dotnet run &
BACKEND_PID=$!

