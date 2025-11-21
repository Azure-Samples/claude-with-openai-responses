# Python - Claude Sonnet 4.5 Example

## Prerequisites

- Python 3.11+
- Azure CLI (logged in)

## Setup

```bash
# Create virtual environment
python -m venv .venv

# Linux shell (Bash, ZSH, etc.), WSL or macOS only
source .venv/bin/activate

# Activate virtual environment (Windows)
.\.venv\Scripts\Activate.ps1

# Install dependencies
pip install -r requirements.txt
```

## Run

```bash
# Run the Responses API example
python claude-openai-responses.py
```

## Files

- `claude-openai-responses.py` - Uses OpenAI SDK with EntraID authentication
- `requirements.txt` - Python dependencies
