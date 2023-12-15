#Backend app de ventas (Api de Asp.net Core)

## Api de Asp.net Core, con los endponit para hacer CRUD de artículos, permite registro,login y manejo de sesiones de usuarios , utiliza JWT.

### Módulo de Crud de artículos
- Crear un artículo.
- Modificar un artículo.
- Listar artículos.
- Eliminar artículos.

### Módulo de usuarios
- Creación de un usuario.
- Login de usuario.
- Gestión de token autenticación de usuario.

### Levantar la aplicación en desarrollo
1. Modificar la cadena de conexion a la BD con el usuario y password de BD en appsettings.json

### Nota: esta api usa base de datos mssqlserver. 

1. Dependencias a instalar:  
```dotnet add package Microsoft.EntityFrameworkCore.SqlServer```
```dotnet add package Microsoft.EntityFrameworkCore.Design```

2. Intalar tools entity framework: ```dotnet tool install --global dotnet-ef```

3. Crear una migracion de la BD: ```dotnet ef migrations add InitialCreate```

4. Actualizar Base de Datos: ```dotnet ef database update```

5. .Levantar un contenedor de docker con mssqlserver user este comando:
```docker run -v BackendVentas:/var/opt/mssql/data -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=PasswordDBAqui" -u 0:0 -p 3306:1433 -d mcr.microsoft.com/mssql/server:2019-latest```

6. Para levantar el server de desarrollo ```dotnet run```
7. Para hacer el build de la app ```ng build``` 
   
