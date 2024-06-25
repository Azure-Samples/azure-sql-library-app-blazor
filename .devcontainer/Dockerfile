# [Choice] .NET version: 6.0-focal, 7.0-bullseye
#RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | sudo gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg

ARG VARIANT="dev-6.0-focal"
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# Add .NET global tools path
ENV PATH $PATH:/home/vscode/.dotnet:/home/vscode/.dotnet/tools

# Import Microsoft Signing Keys
# https://learn.microsoft.com/dotnet/core/install/linux-ubuntu#unable-to-locate--some-packages-could-not-be-installed
RUN sudo apt-get install -y gpg wget \
    && wget https://packages.microsoft.com/keys/microsoft.asc \
    && cat microsoft.asc | gpg --dearmor -o microsoft.asc.gpg \
    && sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
    && sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg

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