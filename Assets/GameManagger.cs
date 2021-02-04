using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Estados
{
    Normal, Dormindo, Fominha, ComFome, Faminto, Cansado, Cansadinho, Exausto, Bolado, Chateado, Tisti
}

public class GameManagger : MonoBehaviour
{

    [Header("Listas de Sprites")]
    public List<Sprite> vitus;
    public List<Sprite> blusas;
    public List<Sprite> toucas;
    public List<Sprite> calcas;
    public List<Sprite> cuequinhas;
    public List<Sprite> backgrounds;
    public List<Sprite> carolzinhas;
    public List<Sprite> comidinhas;

    [Header("Sliders & FillImages")]
    public Slider saudeSlider;
    public Slider felicidadeSlider;
    public Slider energiaSlider;
    public Image saudeFillImage;
    public Image felicidadeFillImage;
    public Image energiaFillImage;

    [Header("Locais de Texto")]
    public TextMeshProUGUI textoFala;
    public TextMeshProUGUI textoFalaCarolzinha;
    public TextMeshProUGUI descansandoFala;
    public TextMeshProUGUI textoSaudeValue;
    public TextMeshProUGUI textoFelicidadeValue;
    public TextMeshProUGUI textoenergiaValue;

    [Header("Grupos de Gameobject")]
    public GameObject acoesGroup;
    public GameObject trocasGroup;
    public GameObject carolzinhaGroup;

    [Header("Sprites")]
    public SpriteRenderer bg;
    public SpriteRenderer calca;
    public SpriteRenderer blusa;
    public SpriteRenderer touca;
    public SpriteRenderer vitu;
    public SpriteRenderer cuequinha;
    public SpriteRenderer carol;
    public SpriteRenderer comida;

    [Header("Variáveis Principais")]
    public int saude;
    public int felicidade;
    public int energia;

    [Header("Animações")]
    public GameObject animacaoCarinho;
    public GameObject animacaoComida;

    [Header("Outras Variáveis")]
    public Estados estadoAtual;
    public List<string> falas;
    public List<string> falasCarolzinha;
    public int saudeMax = 100;
    public int felicidadeMax = 100;
    public int energiaMax = 100;
    public int valorAlimeticio;
    public int valorCarinho;
    public float umSegundo;
    public float tempoFraseNaTela;
    public float tempoTrocaVitu;
    public float tempoTrocaFrase;
    public float tempoMaximoTrocaFrase;
    public float tempoAbaixarEnergia;
    public float tempoAbaixarFelicidade;
    public float tempoAbaixarSaude;
    public float maxTempoAnimacao;
    public int randomComida;

    int indexFalas = 0;
    int indexFalasCarolzinha = 0;
    int indexBackgrounds = 0;
    int indexBlusas = 0;
    int indexToucas = 0;
    int indexCalcas = 0;
    int indexVitus = 0;
    int indexComidas = 0;
    bool acoes;
    bool carolzinha;
    bool trocas;
    bool passouUmSegundo;
    bool descansando;
    //Permite que o sprite do Vitu mude
    bool podeMudarVitu;
    public bool clicou;
    public float tempoUltimoClique;
    public float tempoFraseAcao;
    public float maxTempoFraseAcao;
    bool podePassarFala;
    float tempoDeVitu;
    float tempoComida = 0;
    float tempoCansado = 0;
    float tempoFeliz = 0;
    bool fraseAcao;
    string textoAcao;
    public float tempoAnimacaoCarinho;
    public float tempoAnimacaoComida;
    bool tocandoCarinho;
    bool tocandoComida;
    public string dia;

    // Use this for initialization
    void Awake()
    {
        tempoUltimoClique = 0;
        tempoDeVitu = 0;
        saude = saudeMax;
        felicidade = felicidadeMax;
        energia = energiaMax;
        indexFalas = 0;
        indexFalasCarolzinha = 0;
        textoFala.text = falas[indexFalas];
        textoFalaCarolzinha.text = falasCarolzinha[indexFalasCarolzinha];
        bg.sprite = backgrounds[indexBackgrounds];
        estadoAtual = Estados.Normal;
        podePassarFala = true;
        carolzinha = true;
        //Pega o dia atual e converte pra uma string, a string é formatada a seguir a forma de data Brasileira
        //Pq funciona, não sei
        dia = System.DateTime.Now.ToString("dd/MM/yyyy");
        //Faz a frase relacionada a data ser a ultima frase do rolster de frases
        falas[falas.Count-1] = "No dia "+ dia +" o Vitu te ama.";

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Muda a fala do Vitu
        //MudaFalaVitu();

        //Garantia
        /*
        if (energia > 50 && saude > 50 && felicidade > 50)
        {
            podeMudarVitu = true;
            textoFala.text = falas[indexFalas];
        }*/

        AtualizaBarras();
        ExibeFraseAcao();
        FalasEspeciais();
        TempoDeVitu();
        Morreno();
        TempoProximaFala();
        TrocaStatus();
        PassaFalaAuto();
        TocandoAnimacoes();

        if (clicou)
        {
            tempoUltimoClique += Time.deltaTime;
        }

        if (energia == energiaMax)
        {
            podeMudarVitu = true;
        }

        if (estadoAtual == Estados.Dormindo)
        {
            Descansando();
            descansandoFala.text = "Acordar";
        }
        else if (estadoAtual != Estados.Dormindo)
        {
            Descansando();
            descansandoFala.text = "Mimir";
        }

        if (energia >= energiaMax)
        {
            descansando = false;
        }
    }

    public void MudaFalaVitu()
    {

        if (podePassarFala)
        {
            //Muda a fala
            indexFalas++;
            if (indexFalas >= falas.Count)
            {
                indexFalas = 0;
            }
            clicou = true;
            podePassarFala = false;
            tempoTrocaFrase = 0;
            textoFala.text = falas[indexFalas];
        }
    }

    public void TempoProximaFala()
    {
        if(tempoUltimoClique >= tempoFraseNaTela)
        {
            clicou = false;
            tempoUltimoClique = 0;
            podePassarFala = true;
        }
    }

    //Automaticamente passa a frase do Vitu se não for clicado
    public void PassaFalaAuto()
    {
        if (podePassarFala)
        {
            tempoTrocaFrase += Time.deltaTime;
            if(tempoTrocaFrase >= tempoMaximoTrocaFrase)
            {
                MudaFalaVitu(); 
            }
        }
    }

    public void MostraAcoes()
    {
        carolzinha = !carolzinha;
        carolzinhaGroup.SetActive(carolzinha);
        acoes = !acoes;
        acoesGroup.SetActive(acoes);
    }

    public void MostraTrocas()
    {
        trocas = !trocas;
        trocasGroup.SetActive(trocas);
    }

    public void MudaFraseCarol()
    {
        indexFalasCarolzinha++; 
        if (indexFalasCarolzinha >= falasCarolzinha.Count)
        {
            indexFalasCarolzinha = 0;
        }
        textoFalaCarolzinha.text = falasCarolzinha[indexFalasCarolzinha];
    }

    public void TrocaBlusa()
    {
        indexBlusas++;
        if (indexBlusas == 9 || indexBlusas == 10)
        {
            cuequinha.gameObject.SetActive(true);
            blusa.gameObject.SetActive(false);
            if (indexBlusas == 9)
            {
                cuequinha.sprite = cuequinhas[0];
            }
            if (indexBlusas == 10)
            {
                cuequinha.sprite = cuequinhas[1];
            }
        }
        else
        {
            cuequinha.gameObject.SetActive(false);
            blusa.gameObject.SetActive(true);
        }
        if (indexBlusas >= blusas.Count)
        {
            indexBlusas = 0;
        }
        blusa.sprite = blusas[indexBlusas];
    }

    public void TrocaCalca()
    {
        indexCalcas++;
        if (indexCalcas >= calcas.Count)
        {
            indexCalcas = 0;
        }
        calca.sprite = calcas[indexCalcas];
    }

    public void TrocaTouca()
    {
        indexToucas++;
        if (indexToucas >= toucas.Count)
        {
            indexToucas = 0;
        }
        touca.sprite = toucas[indexToucas];
    }

    public void TrocaBg()
    {
        indexBackgrounds++;
        if (indexBackgrounds >= backgrounds.Count)
        {
            indexBackgrounds = 0;
        }
        bg.sprite = backgrounds[indexBackgrounds];
    }

    public void CarinhoAnimacao()
    {
        animacaoCarinho.SetActive(true);
        tocandoCarinho = true;
        podeMudarVitu = false;
        //Vitu Carinho
        vitu.sprite = vitus[8];
    }

    public void ComidaAnimacao()
    {
        randomComida = UnityEngine.Random.Range(0, comidinhas.Count);
        comida.sprite = comidinhas[randomComida];
        animacaoComida.SetActive(true);
        tocandoComida = true;
        podeMudarVitu = false;
    }

    public void TocandoAnimacoes()
    {
        if (tocandoCarinho)
        {
            tempoAnimacaoCarinho += Time.deltaTime;
            if (tempoAnimacaoCarinho >= maxTempoAnimacao)
            {
                animacaoCarinho.SetActive(false);
                tempoAnimacaoCarinho = 0;
                tocandoCarinho = false;
                podeMudarVitu = true;
            }
        }

        else if (tocandoComida)
        {
            tempoAnimacaoComida += Time.deltaTime;
            if (tempoAnimacaoComida >= maxTempoAnimacao)
            {
                animacaoComida.SetActive(false);
                tempoAnimacaoComida = 0;
                tocandoComida = false;
                podeMudarVitu = true;
            }
        }
    }

    public void Alimentar()
    {
        if (saude == saudeMax)
        {
            fraseAcao = true;
            textoAcao = "Num quer comer agora";
            return;
        }
        else
        {
            fraseAcao = true;
            textoAcao = "Totoso";
            saude += valorAlimeticio;
            ComidaAnimacao();
        }
        if (saude >= saudeMax)
        {
            saude = saudeMax;
        }
    }

    public void DarCarinho()
    {
        CarinhoAnimacao();
        felicidade += valorCarinho;
        fraseAcao = true;
        textoAcao = "Vitu gosta carinho";

        if (felicidade >= felicidadeMax)
        {
            felicidade = felicidadeMax;
        }
    }

    public void ExibeFraseAcao()
    {
        if (fraseAcao)
        {
            //Tempo que a frase da ação vai ficar na tela
            tempoFraseAcao += Time.deltaTime;
            textoFala.text = textoAcao;
            podePassarFala = false;
            tempoTrocaFrase = 0;
        }
        if(tempoFraseAcao > maxTempoFraseAcao)
        {
            //Reseta os contadores
            fraseAcao = false;
            podePassarFala = true;
            tempoUltimoClique = 0;
            tempoTrocaFrase = 0;
            tempoFraseAcao = 0;
            MudaFalaVitu();
        }
    }

    public void Mimir()
    {
        if (estadoAtual == Estados.Dormindo && energia > 50)
        {
            //descansando = false;
            estadoAtual = Estados.Normal;
            fraseAcao = true;
            textoAcao = "Cordi";
            //textoFala.text = "Cordi";
            vitu.sprite = vitus[2];
            //Muda Sprite da Carolzinha pro dela acordada
            carol.sprite = carolzinhas[0];
            //
            textoFalaCarolzinha.text = falasCarolzinha[indexFalasCarolzinha];
        }
        else if (energia >= energiaMax / 2)
        {
            fraseAcao = true;
            textoAcao = "Num quer mimir agora.";
            //textoFala.text = "Num quer mimir agora.";
            //descansando = false;
        }
        else
        {
            estadoAtual = Estados.Dormindo;
            //Muda Sprite da Carolzinha pro dela mimindo
            carol.sprite = carolzinhas[1];
            textoFalaCarolzinha.text = "zZzzZZ";
        }
    }

    //Função que bota o Vitu pra dormir
    public void Descansando()
    {
        if (estadoAtual == Estados.Dormindo)
        {
            umSegundo += Time.deltaTime;
            //A cada 1 segundo aumenta em 1 ponto a barra de energia
            if (umSegundo >= 1)
            {
                umSegundo = 0;
                energia += 1;
            }
            //Se a barra chega ao máximo, para de descansar
            if (energia >= energiaMax)
            {
                energia = energiaMax;
                estadoAtual = Estados.Normal;
                //descansando = false;
            }
        }
    }

    public void AtualizaBarras()
    {
        //Saude
        saudeSlider.maxValue = saudeMax;
        saudeSlider.value = saude;
        textoSaudeValue.text = saude + "/" + saudeMax;
        //Garante que o valor vai ficar entre 0 e o valor maximo definido
        Mathf.Clamp(saude, 0, saudeMax);
        //Faz com que a cor vá mudando no intervalo entre duas cores
        //Quanto mais próximo dos extremos, mais acentuada fica a cor
        saudeFillImage.color = Color.Lerp(Color.red, Color.green, ((float)saude / (float)saudeMax));

        //Felicidade
        felicidadeSlider.maxValue = felicidadeMax;
        felicidadeSlider.value = felicidade;
        textoFelicidadeValue.text = felicidade + "/" + felicidadeMax;
        //Garante que o valor vai ficar entre 0 e o valor maximo definido
        Mathf.Clamp(felicidade, 0, felicidadeMax);
        //Faz com que a cor vá mudando no intervalo entre duas cores
        //Quanto mais próximo dos extremos, mais acentuada fica a cor
        felicidadeFillImage.color = Color.Lerp(Color.magenta, Color.green, ((float)felicidade / (float)felicidadeMax));

        //Cansaço
        energiaSlider.maxValue = energiaMax;
        energiaSlider.value = energia;
        textoenergiaValue.text = energia + "/" + energiaMax;
        //Garante que o valor vai ficar entre 0 e o valor maximo definido
        Mathf.Clamp(energia, 0, energiaMax);
        //Faz com que a cor vá mudando no intervalo entre duas cores
        //Quanto mais próximo dos extremos, mais acentuada fica a cor
        energiaFillImage.color = Color.Lerp(Color.blue, Color.cyan, ((float)energia / (float)energiaMax));
    }

    public void FalasEspeciais()
    {

        if (fraseAcao)
        {
            return;
        }

        if (estadoAtual == Estados.Dormindo)
        {
            podeMudarVitu = false;
            podePassarFala = false;
            textoFala.text = "ZzZzZzZzZzZzZz";
            //Vitu mimindo sprite
            vitu.sprite = vitus[4];
        }

        //Libera a troca de falas e expressões quando no estado normal
        if (estadoAtual == Estados.Normal)
        {
            podeMudarVitu = true;
        }

        #region Falas relacionadas a ENERGIA (CANSAÇO)

        else if (estadoAtual == Estados.Cansadinho)
        {
            textoFala.text = "To cansadinho";
            //Trava a fala nessa fala
            podePassarFala = false;
        }

        else if (estadoAtual == Estados.Cansado)
        {
            textoFala.text = "To cansado";
            //Tisti
            vitu.sprite = vitus[7];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        else if (estadoAtual == Estados.Exausto)
        {
            textoFala.text = "Quer mimir";
            //Tisti
            vitu.sprite = vitus[7];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        #endregion

        #region Falas relacionadas a SAUDE (FOME)

        else if (estadoAtual == Estados.Fominha)
        {
            textoFala.text = "Toco fome";
            //Brabo
            vitu.sprite = vitus[5];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        else if (estadoAtual == Estados.ComFome)
        {
            textoFala.text = "Toco muita fome";
            //Muito Brabo
            vitu.sprite = vitus[6];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        else if (estadoAtual == Estados.Faminto)
        {
            textoFala.text = "Mim dê alimento ç.ç";
            //Tisti
            vitu.sprite = vitus[7];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        #endregion

        #region Falas relacionadas a FELICIDADE (CARINHO)

        else if (estadoAtual == Estados.Bolado)
        {
            textoFala.text = "Queria uns carinho";
            //Trava a fala nessa fala
            podePassarFala = false;
        }

        else if (estadoAtual == Estados.Chateado)
        {
            textoFala.text = "Mim dá carinho, pu favor";
            //Tisti
            vitu.sprite = vitus[7];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }


        else if (estadoAtual == Estados.Tisti)
        {
            textoFala.text = "Eu vai chorar, mim dá um amor.";
            //Tisti
            vitu.sprite = vitus[7];
            //Trava a fala nessa fala
            podePassarFala = false;
            //Trava o Vitu nesse sprite
            podeMudarVitu = false;
        }

        #endregion
    }

    public void TrocaStatus()
    {

        if(estadoAtual == Estados.Dormindo)
        {
            return;
        }

        //saude, felicidade e energia

        if(energia > 50 && saude > 50 && felicidade > 50)
        {
            estadoAtual = Estados.Normal;
            return;
        }

        //Frases relacionadas a energia (Cansaço)

        if (energia <= 50 && energia > 25)
        {
            estadoAtual = Estados.Cansadinho;
            return;
        }

        else if (energia <= 25 && energia > 10)
        {
            estadoAtual = Estados.Cansado;
            return;
        }

        else if (energia <= 10)
        {
            estadoAtual = Estados.Exausto;
            return;
        }


        //Frases relacionadas a saude (Fome)

        if (saude <= 50 && saude > 25)
        {
            estadoAtual = Estados.Fominha;
            return;
        }

        else if (saude <= 25 && saude > 10)
        {
            estadoAtual = Estados.ComFome;
            return;
        }

        else if (saude <= 10)
        {
            estadoAtual = Estados.Faminto;
            return;
        }


        //Frases relacionadas a felicidade (Carinho)


        if (felicidade <= 50 && felicidade > 25)
        {
            estadoAtual = Estados.Bolado;
            return;
        }

        else if (felicidade <= 25 && felicidade > 10)
        {
            estadoAtual = Estados.Chateado;
            return;
        }

        else if (felicidade <= 10)
        {
            estadoAtual = Estados.Tisti;
            return;
        }

    }


    //Troca o Sprite do Vitu automaticamente
    public void TempoDeVitu()
    {
        tempoDeVitu += Time.deltaTime;

        if (tempoDeVitu >= tempoTrocaVitu && podeMudarVitu)
        {
            tempoDeVitu = 0;
            vitu.sprite = vitus[indexVitus];
            indexVitus++;
            if (indexVitus >= 4)
            {
                indexVitus = 0;
            }
        }
    }

    //Tempo no qual os status diminuem
    public void Morreno()
    {
        tempoCansado += Time.deltaTime;
        tempoComida += Time.deltaTime;
        tempoFeliz += Time.deltaTime;

        if (tempoFeliz >= tempoAbaixarFelicidade)
        {
            tempoFeliz = 0;
            felicidade--;
        }

        if (tempoCansado >= tempoAbaixarEnergia)
        {
            tempoCansado = 0;
            energia--;
        }

        if (tempoComida >= tempoAbaixarSaude)
        {
            tempoComida = 0;
            saude--;
        }
    }
}
