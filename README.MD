# Escuela de Talentos - Banco Popular Dominicano

_Escuela de Talentos_ es una plataforma interna diseñada para capacitar a los empleados del Banco Popular sobre la cultura, valores, historia y normas que rigen la organización. El sistema permite gestionar módulos temáticos, registrar el progreso del usuario y ofrecer una experiencia educativa interactiva.

---

## 🚀 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- PostgreSQL
- Rider o Visual Studio Code

---

## ⚙️ Configuración y ejecución

1. **Clona el repositorio**


2. Configura la base de datos

Edita el archivo appsettings.json y ajusta la conexión a PostgreSQL:

```json

ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=EscuelaTalentos;Username=postgres;Password=tu_clave"
}
```

1. correr con visual studio o rider , al correr el proyecto se crearan tablas en la base de datos.

# Usuario por defecto

Al ejecutar el proyecto por primera vez, se crea automáticamente un usuario administrador:

Email: admin@admin.com

Contraseña: admin123

Este usuario tiene acceso completo al sistema para gestionar módulos y usuarios.
