# [Choice] .NET version: 6.0-focal, 7.0-bullseye
ARG VARIANT="6.0-focal"
FROM mcr.microsoft.com/devcontainers/dotnet:0-${VARIANT}

# Add .NET global tools path
ENV PATH $PATH:/home/vscode/.dotnet:/home/vscode/.dotnet/tools

# Install additional OS packages
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends software-properties-common

# Install SQL Tools: SQLPackage and sqlcmd
COPY sql/installSQLtools.sh installSQLtools.sh
RUN bash ./installSQLtools.sh \
     && apt-get clean -y && rm -rf /var/lib/apt/lists/* /tmp/library-scripts

# Install Data API Builder for vscode user
RUN su vscode -c "dotnet tool install --global Microsoft.DataApiBuilder" 2>&1

# Install StrawberryShake.Tools for vscode user
RUN su vscode -c "dotnet tool install --global StrawberryShake.Tools" 2>&1