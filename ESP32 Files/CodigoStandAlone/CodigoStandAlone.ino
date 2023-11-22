//Dev: Prof. Dr. Fábio H. Cabrini
//Date: 14/11/2023
//version: 1.0
#include <WiFi.h>
#include <PubSubClient.h> // Importa a Biblioteca PubSubClient

#define TOPICO_SUBSCRIBE    "/TEF/flux010/cmd"        //tópico MQTT de escuta
#define TOPICO_PUBLISH      "/TEF/flux010/attrs"      //tópico MQTT de envio de informações para Broker
#define TOPICO_PUBLISH_2    "/TEF/flux010/attrs/f"    //tópico MQTT de envio de informações para Broker
                                                      //IMPORTANTE: recomendamos fortemente alterar os nomes
                                                      //            desses tópicos. Caso contrário, há grandes
                                                      //            chances de você controlar e monitorar o ESP32
                                                      //            de outra pessoa.
#define ID_MQTT  "fiware_011"

const char* SSID = "Wokwi-GUEST"; // SSID / nome da rede WI-FI que deseja se conectar
const char* PASSWORD = ""; // Senha da rede WI-FI que deseja se conectar

const char* BROKER_MQTT = "46.17.108.131"; //URL do broker MQTT que se deseja utilizar
int BROKER_PORT = 1883; // Porta do Broker MQTT

//Variáveis e objetos globais
WiFiClient espClient; // Cria o objeto espClient
PubSubClient MQTT(espClient); // Instancia o Cliente MQTT passando o objeto espClient

//Prototypes
void initSerial();
void initWiFi();
void initMQTT();
void reconectWiFi(); 
void mqtt_callback(char* topic, byte* payload, unsigned int length);
void VerificaConexoesWiFIEMQTT(void);

#define FS300A_PULSE     508         // PULSE / LITRO
#define FS300A_FLOW_RATE 60          // LITRO / MINUTO
const float factor = 58800.5817F / 16670.0F;  // FS300A_FLOW_RATE / FS300A_PULSE

#define interruptPin 5

volatile uint16_t pulse;  // Variável que será incrementada na interrupção
uint16_t count;           // Variável para armazenar o valor atual de pulse

float frequency;          // Frequência calculada a partir de count
float flowRate;           // Taxa de fluxo calculada a partir da frequência

portMUX_TYPE mux = portMUX_INITIALIZER_UNLOCKED;  // Mutex para garantir acesso seguro a pulse

void IRAM_ATTR FlowInterrupt() {
  portENTER_CRITICAL_ISR(&mux);  // Entra em uma seção crítica de interrupção
  pulse++;  // Incrementa a variável pulse de maneira segura
  portEXIT_CRITICAL_ISR(&mux);   // Sai da seção crítica de interrupção
}

void setup() {
    initSerial();
    initWiFi();
    initMQTT();
    delay(5000);
    MQTT.publish(TOPICO_PUBLISH, "s|on");

  pinMode(interruptPin, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(interruptPin), FlowInterrupt, RISING); // Configura a interrupção no pino
}

void initSerial() 
{
    Serial.begin(115200);
}

void initWiFi() 
{
    delay(10);
    Serial.println("------Conexao WI-FI------");
    Serial.print("Conectando-se na rede: ");
    Serial.println(SSID);
    Serial.println("Aguarde");
    reconectWiFi();
}

void initMQTT() 
{
    MQTT.setServer(BROKER_MQTT, BROKER_PORT);   //informa qual broker e porta deve ser conectado
    MQTT.setCallback(mqtt_callback);            //atribui função de callback (função chamada quando qualquer informação de um dos tópicos subescritos chega)
}

void mqtt_callback(char* topic, byte* payload, unsigned int length) 
{
    String msg;
     
    //obtem a string do payload recebido
    for(int i = 0; i < length; i++) 
    {
       char c = (char)payload[i];
       msg += c;
    }
    
    Serial.print("- Mensagem recebida: ");
    Serial.println(msg);
    
    //toma ação dependendo da string recebida:
    //verifica se deve colocar nivel alto de tensão na saída D0:
    //IMPORTANTE: o Led já contido na placa é acionado com lógica invertida (ou seja,
    //enviar HIGH para o output faz o Led apagar / enviar LOW faz o Led acender)
     
}

void reconnectMQTT() 
{
    while (!MQTT.connected()) 
    {
        Serial.print("* Tentando se conectar ao Broker MQTT: ");
        Serial.println(BROKER_MQTT);
        if (MQTT.connect(ID_MQTT)) 
        {
            Serial.println("Conectado com sucesso ao broker MQTT!");
            MQTT.subscribe(TOPICO_SUBSCRIBE); 
        } 
        else
        {
            Serial.println("Falha ao reconectar no broker.");
            Serial.println("Havera nova tentatica de conexao em 2s");
            delay(2000);
        }
    }
}

void reconectWiFi() 
{
    //se já está conectado a rede WI-FI, nada é feito. 
    //Caso contrário, são efetuadas tentativas de conexão
    if (WiFi.status() == WL_CONNECTED)
        return;
         
    WiFi.begin(SSID, PASSWORD); // Conecta na rede WI-FI
     
    while (WiFi.status() != WL_CONNECTED) 
    {
        delay(100);
        Serial.print(".");
    }
   
    Serial.println();
    Serial.print("Conectado com sucesso na rede ");
    Serial.print(SSID);
    Serial.println("IP obtido: ");
    Serial.println(WiFi.localIP());
}

void VerificaConexoesWiFIEMQTT(void)
{
    if (!MQTT.connected()) 
        reconnectMQTT(); //se não há conexão com o Broker, a conexão é refeita
     
     reconectWiFi(); //se não há conexão com o WiFI, a conexão é refeita
}

void loop() {
  char msgBuffer[10];

  VerificaConexoesWiFIEMQTT();
  sprintf(msgBuffer,"%.2f", flowRate);
  Frequency();  // Chama a função principal
  MQTT.publish(TOPICO_PUBLISH_2,msgBuffer);
  MQTT.loop();
}

void Frequency() {
  static unsigned long startTime;
  if (micros() - startTime < 1000000UL ) return;   // Intervalo de 1000 milissegundos (1 segundo)
  startTime = micros();

  portENTER_CRITICAL(&mux);  // Entra em uma seção crítica
  count = pulse;  // Salva o valor atual de pulse e zera pulse
  pulse = 0;
  portEXIT_CRITICAL(&mux);   // Sai da seção crítica

  frequency = count;  // Calcula a frequência
  flowRate = (frequency * factor)/5;  // Calcula a taxa de fluxo

  PlotInfo();  // Exibe as informações no Serial Monitor
}

void PlotInfo() {
  Serial.print("Freq.:= " + String(frequency, 2) + " Hz");
  Serial.println(", FLow:= " + String(flowRate, 3) + " L/min");
}
