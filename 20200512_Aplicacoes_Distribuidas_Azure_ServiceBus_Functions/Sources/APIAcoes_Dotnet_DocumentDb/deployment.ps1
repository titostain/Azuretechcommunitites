$dataHora = Get-Date
'Horario Inicial: ' + $dataHora

$dirPublicacao = "publicacao"
$recurso = "groffeservicebus"
$grupo = "DevWeb"

# Força a exclusão de uma pasta para publicação criada anteriormente
if (Test-Path $dirPublicacao) {
    Remove-Item -Recurse -Force $dirPublicacao
}

# Realiza a publicação
dotnet publish -c release -o $dirPublicacao

# Remove um arquivo compactado com a publicação (caso o mesmo tenha sido
# criado anteriormente)
$arqPublicacao = "publicacao.zip"
if (Test-Path $arqPublicacao) {
    Remove-Item -Force $arqPublicacao
}

# Efetua a compressão da pasta com a publicação
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($dirPublicacao, $arqPublicacao)

# Efetua o deployment utilizando o arquivo compactado (.zip)
az webapp deployment source config-zip -n $recurso -g $grupo --src $arqPublicacao

$dataHora = Get-Date
'Horario Final: ' + $dataHora