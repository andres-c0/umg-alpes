Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RecursosHumanos

Namespace Servicios
    Public Class NominaServicio

        Private ReadOnly _datos As NominaDatos

        Public Sub New()
            _datos = New NominaDatos()
        End Sub

        Public Function Listar() As List(Of Nomina)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Nomina
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Insertar(ByVal entidad As Nomina) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Nomina)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace