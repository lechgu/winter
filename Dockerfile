FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . .
RUN rm -rf Backend/bin Backend/obj
RUN dotnet publish Backend -c release --self-contained -r linux-musl-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true

RUN rm -rf Frontend/obj Frontend/bin Frontend/wwwroot/settings.json
RUN dotnet publish Frontend -c Release

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS final

RUN apk add --no-cache icu-libs \
    && mkdir /static

COPY --from=build /app/Backend/bin/release/net7.0/linux-musl-x64/publish/Backend /usr/local/bin/winter
COPY --from=build /app/Frontend/bin/Release/net7.0/publish/wwwroot/ /static/

ENV PORT=80
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV STATIC_DIR=/static
ENV Logging__LogLevel__Microsoft=Information

CMD ["/usr/local/bin/winter"]