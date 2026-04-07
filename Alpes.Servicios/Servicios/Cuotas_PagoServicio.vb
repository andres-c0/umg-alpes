Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Cuotas_PagoServicio

        Private ReadOnly _datos As Cuotas_PagoDatos

        Public Sub New()
            _datos = New Cuotas_PagoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Cuotas_Pago) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Cuotas_Pago)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cuotas_Pago
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Cuotas_Pago)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Cuotas_Pago)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace