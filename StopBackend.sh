kill_port() {
    local port=$1
    local label=$2
    local pids
    pids=$(lsof -ti :"$port" 2>/dev/null)
    if [ -n "$pids" ]; then
        echo -e "${YELLOW}Port $port ($label) kill (PID: $pids)...${NC}"
        kill $pids 2>/dev/null
        # Attendre la libération du port (max 5 s)
        local i=0
        while lsof -ti :"$port" >/dev/null 2>&1 && [ $i -lt 10 ]; do
            sleep 0.5
            i=$((i + 1))
        done
    fi
}

kill_port 5174 "backend"
