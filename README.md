# Scheduler#
Simulador de escalonamento de processos, escrito em C# com interface gráfica em GTK2, com implementação dos escalonadores: FCFS, SJF com preempção e Round Robin.

# Usando

## Arquivos
Este escalonador esta implementado sobre os seguintes formatos de arquivo.

 - PRB
 - TXT
 - LOG

### PRB - Processes Readable as Block
Este arquivo contentem todas as informações dos processos a serem escalonados. Estruturado em formato JSON possui em si as informações de **nome**, **tempo de chegada** e **tempo de execução** respectivamente.

### TXT - Plain Text File 
O programa da suporte também a arquivos **.txt** através de um conversor implementado junto ao simulador, desde que os mesmo siga a formatação especificada abaixo:

|		Nome		|		Tempo de Chegada		|		Tempo de execução		|
|-----------|-----------------------|-----------------------|
|		PDI_0		|				0				|				15				|
|		PDI_1		|				2				|				3				|
|		PDI_2		|				15				|				8				|

Nesta formatação a primeira informação devera ser o nome do processo, seguida pelo tempo de chegada do mesmo e o total de `clock's` de sua execução. Cada processo e delimitado por uma quebra de linha.

### LOG - Log File
Arquivos de log são utilizados na exportação de dados, este arquivo contem as informações de escalonamento dos processos, neles podemos encontrar as listas de processos escalonados, seu nome, e o `clock` em que executou, e como complemento o tamanho do quantum utilizado na escalonação de processos por Round Robin.
E possível apos a exportação de um arquivo **.log** consumi-lo novamente na forma de gráficos.
#### TXT
No que tange a exportação de dados do escalonador, e possível também exportá-los em **.txt**, mas importante ressaltar que o escalonador em seu estado atual não consegue consumir estas informações assim como o faz com arquivos **.log**.


## Areá de Criação - Criando e editando arquivos
A aba de Criação e dedicada a oferecer um ambiente propicio a criação e edição de arquivos do tipo **.prb**.
### Criando novos processos
{Imagem}
Ao clicar em no botão `Novo` e gerado um novo processo com dados randômicos que mais tarde podem ser editados.
### Editando processos
Para se editar um processo basta apenas um duplo click na propriedade que se deseja editar do processo.
### Excluindo processo
A exclusão de um processo e dada pela sua seleção seguida de um click no botão `Remover`
### Salvando as alterações
Salvar alterações em de um arquivo podem ser feitas de duas formas, através do menu ou atravez do atalho `Ctrl+S`

### Abrindo arquivo de processos
Para abrir um arquivo de processos **.prb** encaminhe-se a aba de `Arquivos` seguido de `Abrir...` no menu.

## Areá de Simulação - Executando os escalonadores e gerando resultados

### Selecionando um escalonador
E possível selecionar qual escalonador se deseja visualizar no gráfico, esta seleção e feita a partir do `ComboBox` selecionando pelo nome o escalonador desejado.

### Trocando para um arquivo já aberto
Todos os seus arquivos de sessão estão listados na `ComboBox` localizada abaixo do menu principal. Você pode alternar de um para outro clicando em um arquivo na lista.

### Exportando os dados gerados
Você pode exportar o arquivo atual clicando em `Simulação` seguido de `Exportar` no menu. Você pode optar por exportar os gráficos de cada um dos escalonadores em separado, junto aos dados de comparação, ou gerar uma pasta onde todos os dados serão exportados. Os dados podem ser exportados na forma de imagens **.png** ou **.svg**, ou arquivos de log **.log** ou **.txt**.

# Código Fonte
## Requisitos

 - Mono 5.18
 - MonoDevelp 7.8
 - GTK-Sharp 2

### Instalação
A instalação pode ser realizada apartir do codigo-fonte ou atravez do pacote [.deb](http://handlebarsjs.com/).
#### Por código-fonte
- Mono e MonoDevelop
