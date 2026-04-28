Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RRHH

Namespace Servicios
    Public Class NominaDetalleServicio

        Private ReadOnly _datos As NominaDetalleDatos

        Public Sub New()
            _datos = New NominaDetalleDatos()
        End Sub

        Public Function Insertar(ByVal entidad As NominaDetalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As NominaDetalle)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As NominaDetalle
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of NominaDetalle)
            Return _datos.Listar()
        End Function

    End Class
End Namespace