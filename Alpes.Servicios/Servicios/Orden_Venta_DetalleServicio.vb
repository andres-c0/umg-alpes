Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Orden_Venta_DetalleServicio

        Private ReadOnly _datos As Orden_Venta_DetalleDatos

        Public Sub New()
            _datos = New Orden_Venta_DetalleDatos()
        End Sub

        Public Function Listar() As List(Of Orden_Venta_Detalle)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Orden_Venta_Detalle
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Orden_Venta_Detalle)
            Return _datos.Buscar(valor)
        End Function

        Public Function Insertar(ByVal entidad As Orden_Venta_Detalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Orden_Venta_Detalle)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace
