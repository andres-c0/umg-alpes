Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RecursosHumanos

Namespace Servicios
    Public Class Incidente_LaboralServicio

        Private ReadOnly _datos As Incidente_LaboralDatos

        Public Sub New()
            _datos = New Incidente_LaboralDatos()
        End Sub

        Public Function Listar() As List(Of Incidente_Laboral)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Incidente_Laboral
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Insertar(ByVal entidad As Incidente_Laboral) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Incidente_Laboral)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace