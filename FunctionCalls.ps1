$sources = @("OperaDub", "OperaJBB", "OperaJdm", "OperaJeh", "OperaJfh", "OperaJgv", "OperaJhs", "OperaJMB", "OperaJPP", "OperaJps", "OperaJvm", "OperaLon")

#business date
$BusinessDate = [DateTime]::Today.AddDays(-1).ToString("yyyy-MM-dd")

$profiles = @()
$sources | ForEach-Object{
    $db = $_
    $pro = ProcessProfileDataBase $db, $BusinessDate
    $profiles += $pro
    $pro
}
$profiles | Select-Object Profile, Adresses, Contacts | Export-Csv "$BusinessDate.csv"

Function AddProfileAddress( $database, $date, $profile ) {
    "use $database and $date for adress on $profile.Profile"
}

Function AddProfileContacts( $database, $date, $profile ) {
    "use $database and $date for contacts on $profile.Profile"
}

Function ProcessProfileDataBase( $database, $date ) {
    $processedProfiles = @()
    $profileList = @("use $database and $date")
    $profileList | ForEach-Object {
        $identity = $_
        $currentProfile = New-Object PSobject
        Add-Member -InputObject $currentProfile -MemberType NoteProperty -Name Profile -Value $identity
        Add-Member -InputObject $currentProfile -MemberType NoteProperty -Name Address -Value $identity
        Add-Member -InputObject $currentProfile -MemberType NoteProperty -Name Contact -Value $identity
        $currentProfile.Addressed = AddProfileAddress $database  $date $identity
        $currentProfile.Contacts = AddProfileContacts $database $date $identity
        $processedProfiles += $currentProfile
    }
    $processedProfiles
}
