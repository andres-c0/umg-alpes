Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Seguridad

Namespace Servicios
    Public Class SesionServicio

        Private ReadOnly _datos As SesionDatos

        Public Sub New()
            _datos = New SesionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Sesion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Sesion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Sesion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Sesion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Sesion)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace