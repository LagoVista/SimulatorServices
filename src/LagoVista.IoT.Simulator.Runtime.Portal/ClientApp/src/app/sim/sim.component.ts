import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-sim',
  templateUrl: './sim.component.html',
  styleUrls: ['./sim.component.css']
})
export class SimComponent implements OnInit {
  private _hubConnection: HubConnection | undefined;
  public simulators: Simulator[];
  messages: string[] = [];
  message: '';
  _instanceId: string;
  _baseUrl: string;
  _http: HttpClient;

  public currentSimulator: Simulator;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);

    this._http = http;

    this._baseUrl = baseUrl;
    http.get<Simulator[]>(baseUrl + 'api/simnetwork/simulators').subscribe(result => {
      console.log(result);
      this.simulators = result;
    }, error => console.error(error));

  }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/realtime')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    this._hubConnection.on('notification', (json: string) => {
      const data = JSON.parse(json);

      if (data.channelId === this._instanceId) {
        this.messages.splice(0, 0, data.title);
      }

      const instance = this.simulators.find(sim => sim.instanceId === data.channelId);
      instance.lastUpdate = data.dateStamp;
      if (data.payloadType === 'SimulatorState') {
        console.log(data.payloadJSON);
        instance.currentState = JSON.parse(data.payloadJSON);
        console.log(instance.currentState);
      }

      if (data.payloadType === 'SimulatorStatus') {
        console.log(data.payloadJSON);
        instance.status = JSON.parse(data.payloadJSON);
        console.log(instance.status);
      }
    });

    this._hubConnection.on('Send', (data: any) => {
      const received = `From Send: ${data}`;
      console.log(received);
      this.messages.push(received);
    });
  }

  selectSim(instanceId: string) {
    this._instanceId = instanceId;

    this.currentSimulator = this.simulators.find(sim => sim.instanceId === instanceId);
    console.log(this.currentSimulator);

    this.messages = [];
  }

  changeState(stateId: string) {
    this._hubConnection.send('setState', this.currentSimulator.instanceId, stateId);
  }

  start(instanceId: string) {
    this._hubConnection.send('start', instanceId);
  }

  stop(instanceId: string) {
    this._hubConnection.send('stop', instanceId);
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
  key: string;
  name: string;
  description: string;
  dynamicAttributes: MessageDynamicAttribute[];
}

interface SimulatorStatus {
  isRuning: boolean;
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
