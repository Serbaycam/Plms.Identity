================================================================================
DATABASE UPDATE INSTRUCTIONS / VERÝTABANI GÜNCELLEME TALÝMATLARI
================================================================================

[EN] ENGLISH INSTRUCTIONS
--------------------------------------------------------------------------------
To apply the latest Entity Framework migrations and update the databases, follow these steps:

1. Open your terminal or command prompt.
2. Navigate to the root directory of the solution (the folder containing 'AuthServer.sln').
3. Run the following commands to update each microservice's database:

# For Identity API (Users, Roles, Permissions):
dotnet ef database update --project AuthServer.Identity.API


--------------------------------------------------------------------------------

[TR] TÜRKÇE TALÝMATLAR
--------------------------------------------------------------------------------
En son Entity Framework migrasyonlarýný uygulamak ve veritabanlarýný güncellemek için þu adýmlarý izleyin:

1. Terminalinizi veya komut satýrýnýzý açýn.
2. Solution dosyasýnýn ('AuthServer.sln') bulunduðu ana dizine gidin.
3. Her bir mikroservisin veritabanýný güncellemek için aþaðýdaki komutlarý çalýþtýrýn:

# Identity API için (Kullanýcýlar, Roller, Yetkiler):
dotnet ef database update --project AuthServer.Identity.API


================================================================================
NOTE / NOT:
If you receive a "command not found" error, ensure that the EF Core tool is installed:
"Komut bulunamadý" hatasý alýrsanýz, EF Core aracýnýn yüklü olduðundan emin olun:

dotnet tool install --global dotnet-ef
================================================================================