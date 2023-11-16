//Dev: Prof. Dr. Fábio H. Cabrini
//Date: 14/11/2023
//version: 1.0
#define FS300A_PULSE     508         // PULSE / LITRO
#define FS300A_FLOW_RATE 60          // LITRO / MINUTO
const float factor = 60.0F / 508.0F;  // FS300A_FLOW_RATE / FS300A_PULSE

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
  Serial.begin(115200);

  pinMode(interruptPin, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(interruptPin), FlowInterrupt, CHANGE);  // Configura a interrupção no pino
}

void loop() {
  Frequency();  // Chama a função principal
}

void Frequency() {
  static unsigned long startTime;
  if (micros() - startTime < 1000000UL ) return;   // Intervalo de 1000 milissegundos (1 segundo)
  startTime = micros();

  portENTER_CRITICAL(&mux);  // Entra em uma seção crítica
  count = pulse;  // Salva o valor atual de pulse e zera pulse
  pulse = 0;
  portEXIT_CRITICAL(&mux);   // Sai da seção crítica

  frequency = count / 2.0f;  // Calcula a frequência
  flowRate = frequency * factor;  // Calcula a taxa de fluxo

  PlotInfo();  // Exibe as informações no Serial Monitor
}

void PlotInfo() {
  Serial.print("Freq.:= " + String(frequency, 2) + " Hz");
  Serial.println(", FLow:= " + String(flowRate, 3) + " L/min");
}
