Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Seguridad

Namespace Servicios
    Public Class UsuarioServicio

        Private ReadOnly _datos As UsuarioDatos

        Public Sub New()
            _datos = New UsuarioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Usuario) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Usuario)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Usuario
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Usuario)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Usuario)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace