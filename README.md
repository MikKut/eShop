#!!!!!SETUP STEPS!!!!!

 Update host file on your PC 
   like this instruction https://www.nublue.co.uk/guides/edit-hosts-file/#:~:text=In%20Windows%2010%20the%20hosts,%5CDrivers%5Cetc%5Chosts.
   
Need to path these lines

    127.0.0.1 www.petshop.com
    0.0.0.0 www.petshop.com
    194.168.0.1 www.petshop.com

#login
1) username:bob, password:bob
2) username:alice, password:alice

#docker
docker-compose build --no-cache

docker-compose up

#Add-Migration
dotnet ef --startup-project Catalog/Catalog.Host migrations add InitialMigration --project Catalog/Catalog.Host

#Update-Migration
dotnet ef --startup-project Catalog/Catalog.Host database update InitialMigration --project Catalog/Catalog.Host

#Remove-Migration
dotnet ef --startup-project Catalog/Catalog.Host migrations remove --project Catalog/Catalog.Host -f
