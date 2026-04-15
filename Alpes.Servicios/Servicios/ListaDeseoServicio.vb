Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class ListaDeseosServicio

        Private ReadOnly _datos As ListaDeseosDatos

        Public Sub New()
            _datos = New ListaDeseosDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ListaDeseos) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ListaDeseos) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal listaDeseosId As Integer) As Boolean
            Return _datos.Eliminar(listaDeseosId)
        End Function

        Public Function ObtenerPorId(ByVal listaDeseosId As Integer) As ListaDeseos
            Return _datos.ObtenerPorId(listaDeseosId)
        End Function

        Public Function Listar() As List(Of ListaDeseos)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ListaDeseos)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace