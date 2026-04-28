Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class IncidenteLaboralServicio

        Private ReadOnly _datos As IncidenteLaboralDatos

        Public Sub New()
            _datos = New IncidenteLaboralDatos()
        End Sub

        Public Function Insertar(ByVal entidad As IncidenteLaboral) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As IncidenteLaboral)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As IncidenteLaboral
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of IncidenteLaboral)
            Return _datos.Listar()
        End Function

    End Class
End Namespace