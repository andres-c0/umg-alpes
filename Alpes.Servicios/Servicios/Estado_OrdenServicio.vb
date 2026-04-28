Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Estado_OrdenServicio

        Private ReadOnly _datos As Estado_OrdenDatos

        Public Sub New()
            _datos = New Estado_OrdenDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Estado_Orden) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Estado_Orden)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Estado_Orden
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Estado_Orden)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Estado_Orden)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace