FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/
 

COPY filemon.sln ./
COPY Monitor/*.csproj ./Monitor/ 
COPY test/*.csproj    ./test/ 
COPY Exe/*.csproj    ./Exe/ 



RUN dotnet restore

COPY . ./
 

WORKDIR /app
RUN dotnet publish -c Release -o out
 


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

COPY --from=build /app/out . 

ENTRYPOINT ["dotnet", "Exe.dll"]
