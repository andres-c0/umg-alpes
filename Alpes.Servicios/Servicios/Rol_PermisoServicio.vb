Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Seguridad

Namespace Servicios
    Public Class Rol_PermisoServicio

        Private ReadOnly _datos As Rol_PermisoDatos

        Public Sub New()
            _datos = New Rol_PermisoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Rol_Permiso) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Rol_Permiso)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Rol_Permiso
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Rol_Permiso)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Rol_Permiso)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace