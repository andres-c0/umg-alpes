Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Namespace Seguridad

    Public NotInheritable Class PasswordHasher

        Private Sub New()
        End Sub

        Private Const Prefijo As String = "PBKDF2"
        Private Const IteracionesDefecto As Integer = 100000
        Private Const BytesSalt As Integer = 16
        Private Const BytesHash As Integer = 32


        Public Shared Function GenerarPasswordTemporal(Optional ByVal longitud As Integer = 14) As String
            If longitud < 12 Then
                longitud = 12
            End If

            Const minusculas As String = "abcdefghijkmnopqrstuvwxyz"
            Const mayusculas As String = "ABCDEFGHJKLMNPQRSTUVWXYZ"
            Const numeros As String = "23456789"
            Const especiales As String = "!#$%&*?"
            Dim todos As String = minusculas & mayusculas & numeros & especiales
            Dim caracteres As New List(Of Char) From {
                ObtenerCaracterAleatorio(minusculas),
                ObtenerCaracterAleatorio(mayusculas),
                ObtenerCaracterAleatorio(numeros),
                ObtenerCaracterAleatorio(especiales)
            }

            While caracteres.Count < longitud
                caracteres.Add(ObtenerCaracterAleatorio(todos))
            End While

            Return New String(caracteres.OrderBy(Function(c) ObtenerEnteroAleatorio()).ToArray())
        End Function

        Public Shared Function HashPassword(ByVal password As String) As String
            If password Is Nothing Then
                Throw New ArgumentNullException(NameOf(password))
            End If

            Dim salt(BytesSalt - 1) As Byte

            Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
                rng.GetBytes(salt)
            End Using

            Dim hash As Byte()

            Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, IteracionesDefecto, HashAlgorithmName.SHA256)
                hash = pbkdf2.GetBytes(BytesHash)
            End Using

            Return String.Format("{0}${1}${2}${3}", Prefijo, IteracionesDefecto, Convert.ToBase64String(salt), Convert.ToBase64String(hash))
        End Function

        Public Shared Function VerifyPassword(ByVal password As String, ByVal passwordAlmacenado As String) As Boolean
            If password Is Nothing OrElse String.IsNullOrWhiteSpace(passwordAlmacenado) Then
                Return False
            End If

            Dim almacenado As String = passwordAlmacenado.Trim()

            If Not almacenado.StartsWith(Prefijo & "$", StringComparison.OrdinalIgnoreCase) Then
                Return ComparacionConstante(EncodingBytes(almacenado), EncodingBytes(password))
            End If

            Try
                Dim partes() As String = almacenado.Split("$"c)

                If partes.Length <> 4 Then
                    Return False
                End If

                Dim iteraciones As Integer = Convert.ToInt32(partes(1))
                Dim salt As Byte() = Convert.FromBase64String(partes(2))
                Dim hashEsperado As Byte() = Convert.FromBase64String(partes(3))
                Dim hashCalculado As Byte()

                Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, iteraciones, HashAlgorithmName.SHA256)
                    hashCalculado = pbkdf2.GetBytes(hashEsperado.Length)
                End Using

                Return ComparacionConstante(hashEsperado, hashCalculado)
            Catch
                Return False
            End Try
        End Function

        Public Shared Function NecesitaRehash(ByVal passwordAlmacenado As String) As Boolean
            If String.IsNullOrWhiteSpace(passwordAlmacenado) Then
                Return True
            End If

            If Not passwordAlmacenado.Trim().StartsWith(Prefijo & "$", StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If

            Try
                Dim partes() As String = passwordAlmacenado.Trim().Split("$"c)
                If partes.Length <> 4 Then
                    Return True
                End If

                Dim iteraciones As Integer = Convert.ToInt32(partes(1))
                Return iteraciones < IteracionesDefecto
            Catch
                Return True
            End Try
        End Function

        Public Shared Function CumplePolitica(ByVal password As String) As Boolean
            If String.IsNullOrWhiteSpace(password) Then
                Return False
            End If

            If password.Length < 10 Then
                Return False
            End If

            Dim tieneMinuscula As Boolean = Regex.IsMatch(password, "[a-z]")
            Dim tieneMayuscula As Boolean = Regex.IsMatch(password, "[A-Z]")
            Dim tieneNumero As Boolean = Regex.IsMatch(password, "\d")
            Dim tieneEspecial As Boolean = Regex.IsMatch(password, "[^A-Za-z0-9]")

            Return tieneMinuscula AndAlso tieneMayuscula AndAlso tieneNumero AndAlso tieneEspecial
        End Function


        Private Shared Function ObtenerCaracterAleatorio(ByVal valores As String) As Char
            Dim indice As Integer = ObtenerEnteroAleatorio(0, valores.Length)
            Return valores(indice)
        End Function

        Private Shared Function ObtenerEnteroAleatorio(Optional ByVal minimo As Integer = 0, Optional ByVal maximoExclusivo As Integer = Integer.MaxValue) As Integer
            If maximoExclusivo <= minimo Then
                Return minimo
            End If

            Dim bytes(3) As Byte

            Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
                rng.GetBytes(bytes)
            End Using

            Dim valor As Integer = BitConverter.ToInt32(bytes, 0) And Integer.MaxValue
            Return minimo + (valor Mod (maximoExclusivo - minimo))
        End Function

        Private Shared Function EncodingBytes(ByVal valor As String) As Byte()
            Return System.Text.Encoding.UTF8.GetBytes(If(valor, String.Empty))
        End Function

        Private Shared Function ComparacionConstante(ByVal a As Byte(), ByVal b As Byte()) As Boolean
            If a Is Nothing OrElse b Is Nothing Then
                Return False
            End If

            Dim diferencia As Integer = a.Length Xor b.Length
            Dim maximo As Integer = Math.Max(a.Length, b.Length)

            For i As Integer = 0 To maximo - 1
                Dim ba As Byte = If(i < a.Length, a(i), CByte(0))
                Dim bb As Byte = If(i < b.Length, b(i), CByte(0))
                diferencia = diferencia Or (ba Xor bb)
            Next

            Return diferencia = 0
        End Function

    End Class

End Namespace
