Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class CargoServicio

        Private ReadOnly _datos As CargoDatos

        Public Sub New()
            _datos = New CargoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Cargo) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Cargo)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cargo
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Cargo)
            Return _datos.Listar()
        End Function

    End Class
End Namespace