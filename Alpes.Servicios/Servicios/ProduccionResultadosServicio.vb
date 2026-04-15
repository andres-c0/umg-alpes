Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class ProduccionResultadosServicio

        Private ReadOnly _datos As ProduccionResultadosDatos

        Public Sub New()
            _datos = New ProduccionResultadosDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ProduccionResultados) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ProduccionResultados) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal resultadoId As Integer) As Boolean
            Return _datos.Eliminar(resultadoId)
        End Function

        Public Function ObtenerPorId(ByVal resultadoId As Integer) As ProduccionResultados
            Return _datos.ObtenerPorId(resultadoId)
        End Function

        Public Function Listar() As List(Of ProduccionResultados)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ProduccionResultados)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace