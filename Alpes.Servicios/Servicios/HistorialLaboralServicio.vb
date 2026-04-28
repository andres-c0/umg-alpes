Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class HistorialLaboralServicio

        Private ReadOnly _datos As HistorialLaboralDatos

        Public Sub New()
            _datos = New HistorialLaboralDatos()
        End Sub

        Public Function Insertar(ByVal entidad As HistorialLaboral) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As HistorialLaboral)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As HistorialLaboral
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of HistorialLaboral)
            Return _datos.Listar()
        End Function

    End Class
End Namespace