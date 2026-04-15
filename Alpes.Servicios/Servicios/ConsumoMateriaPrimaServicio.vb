Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class ConsumoMateriaPrimaServicio

        Private ReadOnly _datos As ConsumoMateriaPrimaDatos

        Public Sub New()
            _datos = New ConsumoMateriaPrimaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ConsumoMateriaPrima) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ConsumoMateriaPrima) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal consumoId As Integer) As Boolean
            Return _datos.Eliminar(consumoId)
        End Function

        Public Function ObtenerPorId(ByVal consumoId As Integer) As ConsumoMateriaPrima
            Return _datos.ObtenerPorId(consumoId)
        End Function

        Public Function Listar() As List(Of ConsumoMateriaPrima)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ConsumoMateriaPrima)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace