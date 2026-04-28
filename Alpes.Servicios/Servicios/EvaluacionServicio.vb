Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class EvaluacionServicio

        Private ReadOnly _datos As EvaluacionDatos

        Public Sub New()
            _datos = New EvaluacionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Evaluacion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Evaluacion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Evaluacion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Evaluacion)
            Return _datos.Listar()
        End Function

    End Class
End Namespace