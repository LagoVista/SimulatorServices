// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: bdfccbc314af925a74da7127005aad1c22944079c38aaa79a183f8514f123dfb
// IndexVersion: 2
// --- END CODE INDEX META ---
import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

export interface Sensor {
  name: string;
  sensorIndex: number;
  lowToleranceValue: number;
  lowWarningValue: number;
  inToleranceValue: number;
  highWarningValue: number;
  highToleranceValue: number
}

@Component({
  selector: 'app-sim',
  templateUrl: './seawolf-sim.component.html',
  styleUrls: ['./seawolf-sim.component.css']
})
export class SeaWolfSimComponent implements OnInit {
  private _hubConnection: HubConnection | undefined;
  public simulators: Simulator[];
  messages: string[] = [];
  logmessages: string[] = [];
  showDiagnostics = false;
  message: '';
  _instanceId: string;
  _baseUrl: string;
  _http: HttpClient;

  activeSensor: Sensor;

  public currentSimulator: Simulator;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);

    this._http = http;

    this._baseUrl = baseUrl;
    this.loadSimulators();
  }

  loadSimulators() {
    this.simulators = null;
    console.log("Loading Simulators");
    this._http.get<Simulator[]>(this._baseUrl + 'api/simnetwork/simulators').subscribe(result => {
      console.log(result);
      this.simulators = result;
    }, error => console.error(error));
  }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this._baseUrl}realtime`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    this._hubConnection.on('notification', (json: string) => {
      const data = JSON.parse(json);

      if (data.payloadType === 'Simulators') {
        this.simulators = JSON.parse(data.payloadJSON);
        console.log(this.simulators);
        return;
      }

      if (data.channelId === this._instanceId) {
        this.messages.splice(0, 0, data.title);
      }

      if (this.simulators) {
        const instance = this.simulators.find(sim => sim.instanceId === data.channelId);
        instance.lastUpdate = data.dateStamp;
        if (data.payloadType === 'SimulatorState') {
          instance.currentState = JSON.parse(data.payloadJSON);
        }

        if (data.payloadType === 'SimulatorStatus') {
          instance.status = JSON.parse(data.payloadJSON);
        }
      }
    });

    this._hubConnection.on('Error', (data: any) => {
      const received = `ERROR ENTRY: ${data}`;
      console.log(received);
      this.logmessages.push(received);
    });

    this._hubConnection.on('Log', (data: any) => {
      const received = `LOG ENTRY: ${data}`;
      console.log(received);
      this.logmessages.push(received);
    });

    this._hubConnection.on('Send', (data: any) => {
      const received = `From Send: ${data}`;
      console.log(received);
      this.logmessages.push(received);
    });

    this.selectImage('livewell')
  }

  selectSim(instanceId: string) {
    this._instanceId = instanceId;

    this.currentSimulator = this.simulators.find(sim => sim.instanceId === instanceId);

    this.messages = [];
  }

  changeState(stateId: string) {
    this._hubConnection.send('setState', this.currentSimulator.instanceId, stateId);
  }

  startSimulator(instanceId: string) {
    this._hubConnection.send('startSimulator', instanceId);
  }

  stopSimulator(instanceId: string) {
    this._hubConnection.send('stopSimulator', instanceId);
  }

  sendMessage(messageId: string) {
    this._hubConnection.send('sendMessage', this.currentSimulator.instanceId, messageId);
  }

  selectImage(area: string) {
    console.log(area);
    switch (area) {
      case 'ambienttemperature':
        this.activeSensor = { name: 'Outside Temp', lowToleranceValue: 32, lowWarningValue:36, highWarningValue:95,
          highToleranceValue: 100, sensorIndex: 11, inToleranceValue: 78 };
        break;
        case 'livewell':
          this.activeSensor = { name: 'Live Well Temp', lowToleranceValue: 64, lowWarningValue:67, highWarningValue:89,
            highToleranceValue: 92, sensorIndex: 10, inToleranceValue: 80 };
          break;
        case 'starterbattery':
        this.activeSensor = { name: 'Starter Battery', lowToleranceValue: 10.5, lowWarningValue:12.2, highWarningValue:14.8,
          highToleranceValue: 16.2, sensorIndex: 4, inToleranceValue: 13.8 };
        break;
      case 'batteryswitch':
        this.activeSensor = { name: 'Battery Switch', lowToleranceValue: 0, lowWarningValue:-1, highWarningValue:-1,
          highToleranceValue: -1, sensorIndex: 6, inToleranceValue: 1 };
        break;
      case 'coolertemperature':
        this.activeSensor = { name: 'Cooler Temperature', lowToleranceValue: -1, lowWarningValue:-1, highWarningValue:28,
          highToleranceValue: 32, sensorIndex: 12, inToleranceValue: 26 };
        break;
      case 'gps':
        this.activeSensor = { name: 'Location Tracking', lowToleranceValue: 10.5, lowWarningValue:-1, highWarningValue:-1,
        highToleranceValue: 16.2, sensorIndex: 4, inToleranceValue: 13.8 };
        break;
      case 'trollingmotor':
        this.activeSensor = { name: 'Trolling Motor', lowToleranceValue: 36, lowWarningValue:38, highWarningValue:44,
        highToleranceValue: 46, sensorIndex: 5, inToleranceValue: 41.4 };
        break;
        
      case 'highwater':
        this.activeSensor = { name: 'High Water', lowToleranceValue: -1, lowWarningValue:-1, highWarningValue:-1,
        highToleranceValue: 1, sensorIndex: 13, inToleranceValue: 0 };
        break;
        
      case 'vapor':
        this.activeSensor = { name: 'Gas Vapor', lowToleranceValue: -1, lowWarningValue:-1, highWarningValue:-1,
        highToleranceValue: 1, sensorIndex: 9, inToleranceValue: 0 };
        break;

      case 'motion':
        this.activeSensor = { name: 'Motion', lowToleranceValue: -1, lowWarningValue:-1, highWarningValue:-1,
        highToleranceValue: 1, sensorIndex: 14, inToleranceValue: 0 };
        break;
        
      case 'moisture':
        this.activeSensor = { name: 'Moisture', lowToleranceValue: -1, lowWarningValue:-1, highWarningValue:-1,
        highToleranceValue: 1, sensorIndex: 15, inToleranceValue: 0 };
        break;
    }
  }

  sendValue(value: number) {
    this._hubConnection.send('updateSensorValue', this.currentSimulator.instanceId, this.activeSensor.sensorIndex, value);
  }

  reload() {
    this.simulators = null;
    this.currentSimulator = null;
    this._instanceId = null;
    this._hubConnection.send("reload");
  }

  start() {
    this._hubConnection.send("start");
  }

  stop() {
    this._hubConnection.send("stop");
  }

  send() {
    const data = `Sent: ${this.message}`;

    this._http.get<Simulator[]>(this._baseUrl + 'api/simnetwork/Update').subscribe(result => {
    }, error => console.error(error));
  }
}

interface EntityHeader {
  id: string;
  name: string;
}

interface Notification {
  messageId: string;
  dateStamp: string;
  channel: EntityHeader;
  verbosity: EntityHeader;
  channelId: string;
  title: string;
  text: string;
  payloadType: string;
  payloadJSON: string;
}

interface SimulatorState {
  id: string;
  key: string;
  name: string;
  description: string;
}

interface MessageDynamicAttribute {
  id: string;
  name: string;
  key: string;
  defaultValue: string;
  parameterType: EntityHeader;
}

interface Message {
  id: string;
  key: string;
  name: string;
  description: string;
  dynamicAttributes: MessageDynamicAttribute[];
}

interface SimulatorStatus {
  isRunning: boolean;
  text: string;
}

interface Simulator {
  instanceName: string;
  simulatorName: string;
  instanceId: string;
  status: SimulatorStatus;
  currentState: SimulatorState;
  states: SimulatorState[];
  messages: Message[];
  lastUpdate: string;
}
