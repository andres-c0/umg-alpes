Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Seguridad

Namespace Servicios
    Public Class RolServicio

        Private ReadOnly _datos As RolDatos

        Public Sub New()
            _datos = New RolDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Rol) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Rol)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Rol
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Rol)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Rol)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace