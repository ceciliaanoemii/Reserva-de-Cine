# Reserva Cine 馃摉

## Objetivos 馃搵
Desarrollar un sistema, que permita la administraci贸n general del cine (de cara a los empleados): Peliculas, Salas, , Funciones, Clientes, etc., como as铆 tambi茅n, permitir a los clientes, realizar reserva de las funciones ofrecidas.
Utilizar Visual Studio 2019 preferentemente y crear una aplicaci贸n utilizando ASP.NET MVC Core (versi贸n a definir por el docente 2.2 o 3.1).

<hr />

## Enunciado 馃摙
La idea principal de este trabajo pr谩ctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relev贸 e identific贸 la informaci贸n aqu铆 contenida. 
A partir de este momento, deber谩n comprender lo que se est谩 requiriendo y construir dicha aplicaci贸n, 

Deben recopilar todas las dudas que tengan y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, 茅l nos ayudar谩 a conseguir la informaci贸n ya un poco m谩s procesada. 
Es importante destacar, que este proceso, no debe esperar a ser en clase; es importante, que junten algunas consultas, sea de 铆ndole funcional o t茅cnicas, en lugar de cada consulta enviarla de forma independiente.

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relaci贸n del paciente con Turno es 1:1 o 1:N?

2.Est谩 bien que encaremos la validaci贸n del turno activo, con una propiedad booleana en el Turno?

<hr />

### Proceso de ejecuci贸n en alto nivel 鈽戯笍
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/).
 - Adicionar todos los modelos dentro de la carpeta Models cada uno en un archivo separado.
 - Especificar todas las restricciones y validaciones solicitadas a cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Crear las relaciones entre las entidades
 - Crear una carpeta Data que dentro tendr谩 al menos la clase que representar谩 el contexto de la base de datos DbContext. 
 - Crear el DbContext utilizando base de datos en memoria (con fines de testing inicial). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades en el DbContext.
 - Crear el Scaffolding para permitir los CRUD de las entidades al menos solicitadas en el enunciado.
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Realizar un sistema de login con al menos los roles equivalentes a <Usuario Cliente> y <Usuario Administrador> (o con permisos elevados).
 - Si el proyecto lo requiere, generar el proceso de registraci贸n. 
 - Un administrador podr谩 realizar todas tareas que impliquen interacci贸n del lado del negocio (ABM "Alta-Baja-Modificaci贸n" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente> s贸lo podr谩 tomar acci贸n en el sistema, en base al rol que tiene.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos del lado de los permisos.
 - Todo lo referido a la presentaci贸n de la aplicai贸n (cuestiones visuales).
 
<hr />

## Entidades 馃搫

- Usuario
- Cliente
- Empleado
- Reserva
- Funci贸n
- Pelicula
- Sala
- Genero

`Importante: Todas las entidades deben tener su identificador unico. Id o <ClassNameId>`

`
Las propiedades descriptas a continuaci贸n, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como as铆 tambi茅n las restricciones.
`

**Usuario**
```
- Nombre
- Email
- FechaAlta
- Password
```

**Cliente**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email 
- Reservas
```

**Empleado**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email
- Legajo
```

**Pelicula**
```
- FechaLanzamiento
- Titulo
- Descripcion
- Genero
- Funciones
- Duracion *Consultar punto*
```
**Genero**
```
- Nombre
- Peliculas
```

**Sala**
```
- Numero
- TipoSala
- CapacidadButacas
- Funciones
```

**TipoSala**
```
- Nombre
- Precio
```

**Funci贸n**
```
- Fecha
- Hora
- Descripcion
- ButacasDisponibles
- Confirmada
- Pelicula
- Sala
- Reservas
```


**Reserva**
```
- Funcion
- FechaAlta
- Cliente
- CantidadButacas
```

**NOTA:** aqu铆 un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades 鈱笍
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

**Usuario**
- Los clientes pueden auto registrarse.
- La autoregistraci贸n desde el sitio, es exclusiva para los clientes. Por lo cual, se le asignar谩 dicho rol.
- Los empleados, deben ser agregados por otro Empleado.
	- Al momento, del alta del empleado, se le definir谩 un username y password.
    - Tambi茅n se le asignar谩 a estas cuentas el rol de empleado.

**Cliente**
- Un cliente puede realizar una reserva Online
    - El proceso ser谩 en modo Wizard.
        - Selecciona la pelicula
        - Selecciona la cantidad de butacas que quiere reservar.
        - Seleccionar谩 una funci贸n disponible dentro de la oferta.
            - La oferta estar谩 restringida desde el momento de la consulta hasta 7 dias posteriores.
            - Las funciones deben estar confirmadas
            - No debe incluir desde luego funciones que no tenga butacas disponibles.            
            - Debe ser en base a la oferta de la pelicula seleccionada.
            - El cliente, solo puede tener una reserva activa.
        - El cliente, podr谩 en todo momento, ver si tiene o no una reserva para una funci贸n futura.            
            - Podr谩 cancelarla, solo si es hasta 24hs. antes.
- Puede ver las reservas pasadas.
- Puede actualizar datos de contacto, como el telefono, direcci贸n,etc.. Pero no puede modificar su DNI, Nombre, Apellido, etc.

**Empleado**
- El empleado puede listar las reservas por cada funci贸n "en el futuro" o "en el pasado".
- El empleado, puede habilitar o cancelar funciones. 
    - Solo pueden cancelarse, si no tiene reservas.
- Tambi茅n, puede ver un balance de recaudaci贸n por pelicula en mes calendario. 
- Puede dar de alta las Salas, Peliculas, etc. 
    - Nadie, puede eliminar las salas, pero si puede cambiar el tipo.


**Reserva**
- La reserva al crearse debe estar en estado activa.
- El cliente solo puede tener una reserva activa.
- La reserva, tiene que validar, que sea factible, en cuanto a la cantidad de butacas que selecciona al cliente para una funci贸n especifica.
    - Si puede realizar la reserva se debe actualizar las butacas disponibles (Capacidad de la sala vs Reservas realizadas previas y actual).


**Aplicaci贸n General**
- Informaci贸n institucional.
- Se deben listar las peliculas en cartelera sin iniciar sesi贸n.
- Poder reservar, autom谩ticamente desde la una pelicula (con solicitud de inicio de sesi贸n previo si fuese requerido)
- Por cada pelicula, se tiene que poder listar las funciones activas para la proxima semana. 
- La disponibilidad de las funciones, solo puede verse al tener una sesi贸n iniciada como cliente.


`
Nota: Las butacas no son numeradas. El complejo, no tiene limites fisicos en la construcci贸n de salas. Las funciones tienen una duraci贸n fija de 2hs. 
`
