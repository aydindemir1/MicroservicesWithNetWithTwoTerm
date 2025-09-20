# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#
#Bu Dockerfile, bir ASP.NET Core uygulamasýnýn Docker konteyneri içinde inþa edilmesi ve çalýþtýrýlmasý için adým adým bir süreç tanýmlar. Dockerfile, geliþtirme, inþa etme, yayýnlama ve üretim ortamýnda çalýþtýrma için farklý aþamalar içerir. Her bir aþamanýn detaylý açýklamasý þu þekildedir:
#


#### 1. **Base (Temel) Aþamasý (Visual Studio Debug Modu Ýçin)**
#
#```dockerfile
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
# Base image with ASP.NET Core runtime # ASP.NET Core çalýþma zamaný için temel imaj
USER $APP_UID 
# Use the non-root user to run the application inside the container for better security practices  # Uygulama konteyner içinde, daha güvenli bir þekilde çalýþtýrmak için root olmayan bir kullanýcý kullanýlýr
WORKDIR /app 
# Set the working directory inside the container to /app # This is where the application will run # Konteyner içinde çalýþma dizinini /app olarak ayarlayýn
EXPOSE 8080 
# Expose port 8080 for the application to listen on (HTTP) # Uygulamanýn dinleyeceði 8080 portunu açýn (HTTP)


#
#* **`FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base`**: Bu satýr, konteynerin temel imajýný belirtir. ASP.NET Core çalýþma zamaný imajýný (`aspnet:8.0`) kullanýr. 
#Bu imaj, sadece ASP.NET Core uygulamalarýný çalýþtýrmak için gerekli runtime (çalýþma zamaný) ortamýný içerir, geliþtirme ve inþa iþlemleri için gerekli SDK’yý içermez.
#
#* **`USER $APP_UID`**: Uygulama konteyner içinde, root kullanýcýsý yerine daha güvenli bir kullanýcý ile çalýþtýrýlacak þekilde ayarlanýr. `$APP_UID` ortam deðiþkeni, derleme veya çalýþma sýrasýnda belirlenir.
#
#* **`WORKDIR /app`**: Konteyner içindeki çalýþma dizini `/app` olarak ayarlanýr. Bu dizin, uygulamanýn çalýþtýðý yer olacaktýr.
#
#* **`EXPOSE 8080`**: Konteynerin 8080 portu açýlýr. Bu, uygulamanýn HTTP isteklerini bu port üzerinden dinlemesini saðlar. Konteyner çalýþtýrýldýðýnda, bu port host (ana bilgisayar) ile eþleþtirilebilir.
#



#
#### 2. **Build (Ýnþa) Aþamasý (Uygulamanýn Derlenmesi)**
#
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 
# SDK image to build the application   # Uygulama inþa etmek için SDK imajý
ARG BUILD_CONFIGURATION=Release 
# Build configuration argument, default is Release # Derleme yapýlandýrma argümaný, varsayýlan olarak Release
WORKDIR /src
# Set the working directory inside the container to /src for building the application # Konteyner içinde çalýþma dizinini /src olarak ayarlayýn
COPY ["Docker.API.csproj", "."] 
# Copy the project file to the working directory # Proje dosyasýný çalýþma dizinine kopyalayýn
RUN dotnet restore "./Docker.API.csproj"
# Restore dependencies for the project  # Projenin baðýmlýlýklarýný geri yükleyin
COPY . . 
# Copy the entire project to the working directory  # Projenin tüm dosyalarýný çalýþma dizinine kopyalayýn


WORKDIR "/src/." 
# Set the working directory to the project root  # Çalýþma dizinini proje köküne ayarlayýn
RUN dotnet build "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build 
# Build the project in the specified configuration and output to /app/build directory  # Projeyi belirtilen yapýlandýrmaya göre derleyin ve /app/build dizinine çýktýyý oluþturun

#```
#
#* **`FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build`**: Bu satýr, uygulamanýn derlenmesi için gerekli olan .NET SDK imajýný (`sdk:8.0`) kullanýr. SDK imajý, uygulamanýn derlenmesi için gerekli tüm araçlarý (derleyiciler, runtime, vb.) içerir.
#
#* **`ARG BUILD_CONFIGURATION=Release`**: Bu satýr, `BUILD_CONFIGURATION` adlý bir derleme argümaný tanýmlar. Varsayýlan olarak `Release` olarak ayarlanýr, ancak bu ayar deðiþtirilebilir (örneðin, `Debug`).
#
#* **`WORKDIR /src`**: Konteyner içindeki çalýþma dizini `/src` olarak ayarlanýr. Derleme iþlemleri bu dizinde yapýlýr.
#
#* **`COPY ["Docker.API.csproj", "."]`**: Proje dosyasýný (Docker.API.csproj) çalýþma dizinine kopyalar. Bu dosya, baðýmlýlýklarýn geri yüklenmesi ve derlenmesi için gereklidir.
#
#* **`RUN dotnet restore "./Docker.API.csproj"`**: `dotnet restore` komutu çalýþtýrýlýr. Bu, projedeki NuGet baðýmlýlýklarýný indirir ve geri yükler.
#
#* **`COPY . .`**: Projeye ait tüm dosyalar, çalýþma dizinine (`/src`) kopyalanýr. Bu dosyalar, uygulamanýn kaynak kodu, yapýlandýrma dosyalarý ve diðer gerekli içeriklerdir.
#
#* **`WORKDIR "/src/."`**: Çalýþma dizini, projenin kök dizinine (`/src/.`) ayarlanýr.
#
#* **`RUN dotnet build "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build`**: `dotnet build` komutu ile uygulama derlenir. Belirtilen yapýlandýrmaya göre (`Release` veya `Debug`) derlenir ve derleme çýktýlarý `/app/build` dizinine yerleþtirilir.
#


#### 3. **Publish (Yayýnlama) Aþamasý (Uygulamanýn Yayýnlanmasý)**
#
#```dockerfile

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
# Publish stage to create the final output # Yayýnlama aþamasý, final çýktýyý oluþturur
ARG BUILD_CONFIGURATION=Release 
# Build configuration argument, default is Release # Derleme yapýlandýrma argümaný, varsayýlan olarak Release
RUN dotnet publish "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false 
# Publish the project in the specified configuration to /app/publish directory without an app host
# Projeyi belirtilen yapýlandýrmaya göre yayýnlayýn ve /app/publish dizinine yerleþtirin, app host kullanmadan

#```
#
#* **`FROM build AS publish`**: Bu satýr, önceki `build` aþamasýna dayalý olarak bir `publish` aþamasý oluþturur. Burada uygulamanýn yayýnlanmasý iþlemi yapýlacaktýr.
#
#* **`RUN dotnet publish "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false`**: `dotnet publish` komutu ile uygulama yayýnlanýr. Yayýnlanan dosyalar `/app/publish` dizinine kopyalanýr. `/p:UseAppHost=false` parametresi, uygulamanýn native (yerel) bir host dosyasý oluþturmasýný engeller (bu, konteyner ortamý için gereksizdir).
#


#### 4. **Final (Son) Aþama (Uygulamanýn Çalýþtýrýlmasý Ýçin Üretim Konteyneri)**
#
#```dockerfile

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final 
# Final stage to create the runtime image # Son aþama, çalýþma zamaný imajýný oluþturur
WORKDIR /app
# Set the working directory inside the container to /app # Konteyner içindeki çalýþma dizinini /app olarak ayarlayýn
COPY --from=publish /app/publish . 
# Copy the published output from the publish stage to the working directory # Yayýnlanan çýktýyý /app dizinine kopyalayýn
ENTRYPOINT ["dotnet", "Docker.API.dll"] 
# Set the entry point to run the application using dotnet # Uygulamayý çalýþtýrmak için giriþ noktasý olarak dotnet komutunu ayarlayýn

#```
#
#* **`FROM base AS final`**: Bu satýr, daha önce oluþturduðumuz `base` aþamasýndan temel alýr ve final bir aþama oluþturur. Burada, uygulamanýn çalýþtýrýlacaðý üretim ortamý için son imaj oluþturulur.
#
#* **`WORKDIR /app`**: Çalýþma dizini, `/app` olarak ayarlanýr. Uygulama burada çalýþtýrýlacaktýr.
#
#* **`COPY --from=publish /app/publish .`**: Yayýnlama aþamasýndan (`publish`) çýkarýlan dosyalar `/app` dizinine kopyalanýr. Bu dosyalar, uygulamanýn yayýnlanan versiyonunu içerir.
#
#* **`ENTRYPOINT ["dotnet", "Docker.API.dll"]`**: Konteyner çalýþtýðýnda, `dotnet Docker.API.dll` komutu çalýþtýrýlýr. Bu, ASP.NET Core uygulamasýnýn baþlatýlmasýný saðlar.
#

#### Özet
#
#Bu Dockerfile, ASP.NET Core uygulamasýný bir Docker konteynerinde çalýþtýrmak için çok aþamalý bir yapý kullanýr:
#
#1. **Base (Temel)**: Çalýþma zamaný (runtime) imajý olan ASP.NET Core imajý ile baþlar.
#2. **Build (Ýnþa)**: Uygulama, SDK imajý kullanýlarak derlenir.
#3. **Publish (Yayýnlama)**: Yayýnlanacak çýktý dosyalarý oluþturulur.
#4. **Final (Son)**: Uygulama, çalýþma zamaný ortamýnda çalýþtýrýlmaya hazýr hale gelir.
#
#Bu süreç, hem geliþtirme hem de üretim ortamlarý için uygulamanýn verimli ve güvenli bir þekilde konteynerize edilmesini saðlar.

