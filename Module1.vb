Imports System.IO

Module Module1

    Sub Main()
        Try
            ' Ruta del archivo binario temporal
            Dim filePath As String = Path.Combine(Path.GetTempPath(), "open_drawer.bin")

            ' Leer los datos desde el archivo cajon.cg
            ' el archivo cajon.cg en la primera línea debe contener el nombre del servidor y el modelo de la impresora compartida
            ' por ejemplo \\SERVIDOR\CITIZEN
            ' la segunda línea los códigos de apertura en hexadecimal
            ' por ejemplo 1B 70 00 19 FA (estos códigos son para la CITIZEN Model TZ30-M01)
            Dim configFilePath As String = "cajon.cg"
            Dim printerPath As String
            Dim commandHex As String()

            ' Verificar si el archivo de configuración existe
            If File.Exists(configFilePath) Then
                Dim configLines As String() = File.ReadAllLines(configFilePath)
                If configLines.Length < 2 Then
                    Throw New Exception("El archivo de configuración 'cajon.cg' no contiene suficientes líneas.")
                End If

                ' Leer la ruta de la impresora
                printerPath = configLines(0).Trim()

                ' Leer y convertir los códigos hexadecimales a byte array
                commandHex = configLines(1).Trim().Split(" "c)
                Dim openDrawerCommand As Byte() = Array.ConvertAll(commandHex, Function(hex) Convert.ToByte(hex, 16))

                ' Escribir el comando en el archivo binario
                File.WriteAllBytes(filePath, openDrawerCommand)

                ' Copiar el archivo a la impresora
                File.Copy(filePath, printerPath)

                ' Eliminar el archivo temporal
                File.Delete(filePath)
            Else
                Throw New FileNotFoundException("El archivo de configuración 'cajon.cg' no fue encontrado.")
            End If
        Catch ex As Exception
            Console.WriteLine("Error al abrir el cajón: " & ex.Message)
        End Try
    End Sub

End Module
