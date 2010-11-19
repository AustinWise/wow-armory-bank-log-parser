Imports System.IO
Imports System.Xml.XPath
Imports Austin.Net

Module Program

    Private ReadOnly saveLocation As String = "D:\bankdata\"
    Private ReadOnly realm As String = "Azgalor"
    Private ReadOnly guild As String = "Azgalor NaReuTo FaNcLuB"
    Private userName As String
    Private dlm As IDownloadManager = New DownloadManager()

    Sub Main()
        Console.WriteLine("Please enter your username: ")
        userName = Console.ReadLine()
        Console.WriteLine("Please enter your password: ")
        login(userName, Console.ReadLine())
        'Console.WriteLine(dlm.DownloadString("http://www.wowarmory.com/login-status.xml"))
        downloadData()
    End Sub

    Private Sub downloadData()
        Dim prev As Integer = 0
        Dim now As Integer = 0
        Dim group As Integer = 1

        While Not now = -1
            Dim nextString As String = downloadAndGetNext(group, now, prev)

            Console.WriteLine("Downloaded " + group.ToString())

            prev = now
            If Not Integer.TryParse(nextString, now) Then
                now = -1
            End If
            group += 1
        End While
    End Sub

    Private Function downloadAndGetNext(ByVal group As Integer, ByVal now As Integer, ByVal prev As Integer) As String
        Dim req As New ArmoryDownloadRequest(New Uri(String.Format("http://www.wowarmory.com/vault/guild-bank-log.xml?r={0}&n={1}&group={2}&now={3}&prev={4}", realm, guild, group, now, prev)))

        Dim page As String = dlm.DownloadString(req)
        File.WriteAllText(Path.Combine(saveLocation, group.ToString() + ".xml"), page)
        Return getNext(page)
    End Function

    Private Function getNext(ByVal page As String) As String
        Dim doc As New XPathDocument(New StringReader(page))
        Dim nav As XPathNavigator = doc.CreateNavigator()
        Return nav.SelectSingleNode("/page/guildInfo/guildBank/banklogs/@next").Value
    End Function

    Private Sub login(ByVal username As String, ByVal password As String)
        Dim prereq As New ArmoryDownloadRequest(New Uri("https://us.battle.net/login/en/login.xml?ref=http%3A%2F%2Fwww.wowarmory.com%2Findex.xml&app=armory"))
        prereq.SaveCookiesReturnedByServer = True
        dlm.DownloadString(prereq)

        Dim req As New ArmoryDownloadRequest(New Uri("https://us.battle.net/login/en/login.xml?app=armory&ref=http%3A%2F%2Fwww.wowarmory.com%2Findex.xml"))
        req.SaveCookiesReturnedByServer = True
        req.PostValues.Add("accountName", username)
        req.PostValues.Add("password", password)
        req.PostValues.Add("persistLogin", "on")
        dlm.DownloadString(req)
    End Sub

End Module
