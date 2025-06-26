#!/usr/bin/env bash
set -euo pipefail
IFS=$'\n\t'

#------------------------------------------------------------------------------
# 1) Core prerequisites (apt)
#------------------------------------------------------------------------------
sudo apt-get update
sudo apt-get install -y \
  curl wget gnupg ca-certificates lsb-release apt-transport-https mono-devel

#------------------------------------------------------------------------------
# 2) .NET 9 SDK installation
#------------------------------------------------------------------------------
UBUNTU_VERSION="$(lsb_release -rs)"
if [[ "${UBUNTU_VERSION}" =~ ^(20\.04|22\.04)$ ]]; then
  echo "Registering Microsoft feed for Ubuntu ${UBUNTU_VERSION}..."
  wget -q \
    "https://packages.microsoft.com/config/ubuntu/${UBUNTU_VERSION}/packages-microsoft-prod.deb" \
    -O packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  rm packages-microsoft-prod.deb
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-9.0
else
  echo "Ubuntu ${UBUNTU_VERSION} not in Microsoft feed; using dotnet-install.sh..."
  curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
  chmod +x dotnet-install.sh
  ./dotnet-install.sh --channel 9.0
  rm dotnet-install.sh
fi

# ensure dotnet is on PATH in this session
export DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}"
export PATH="$PATH:${DOTNET_ROOT}:${DOTNET_ROOT}/tools"

echo ".NET version: $(dotnet --version)"

#------------------------------------------------------------------------------
# 3) Node.js 18 installation
#------------------------------------------------------------------------------
if ! command -v node >/dev/null || ! node --version | grep -q '^v18'; then
  echo "Installing Node.js 18..."
  curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
  sudo apt-get install -y nodejs
fi

echo "Node.js version: $(node --version)"

#------------------------------------------------------------------------------
# 4) Python environment & dependencies
#------------------------------------------------------------------------------
if [[ -f "pyproject.toml" ]]; then
  echo "Found pyproject.toml -> using Poetry-managed venv"
  if ! command -v poetry >/dev/null; then
    echo "Installing Poetry (user mode)..."
    pip3 install --user poetry
    export PATH="$PATH:$HOME/.local/bin"
  fi
  poetry config virtualenvs.in-project true
  poetry install --with test

elif [[ -f "requirements.txt" ]]; then
  echo "Found requirements.txt -> creating .venv"
  python3 -m venv .venv
  # shellcheck disable=SC1091
  source .venv/bin/activate
  pip install --upgrade pip
  pip install -r requirements.txt

else
  echo "No Python project detected; skipping Python setup."
fi

#------------------------------------------------------------------------------
# 5) JavaScript packages (pnpm/npm)
#------------------------------------------------------------------------------
if [[ -f "package.json" ]]; then
  echo "package.json detected -> installing JS deps"
  if ! command -v pnpm >/dev/null; then
    echo "Installing pnpm globally..."
    npm install -g pnpm
  fi
  pnpm install
else
  echo "No package.json found; skipping JS install."
fi

#------------------------------------------------------------------------------
# 6) ABP CLI
#------------------------------------------------------------------------------
if ! command -v abp >/dev/null; then
  echo "Installing ABP CLI..."
  dotnet tool install -g Volo.Abp.Cli
else
  echo "ABP CLI already present."
fi

#------------------------------------------------------------------------------
# 7) Persist env‐vars for future shells
#------------------------------------------------------------------------------
cat << 'EOF_BASHRC' >> ~/.bashrc
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$HOME/.dotnet:$HOME/.dotnet/tools:$HOME/.local/bin"
EOF_BASHRC

echo "All done!"
