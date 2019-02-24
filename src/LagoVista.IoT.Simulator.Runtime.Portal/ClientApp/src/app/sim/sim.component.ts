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
      let data = JSON.parse(json);

      if(data.channelId === this._instanceId) {
        this.messages.splice(0, 0, data.title);
      }
      
      var intsance = this.simulators.find(sim=>sim.instanceId == data.channelId);
      intsance.lastUpdate = data.dateStamp;
    });

    this._hubConnection.on('Send', (data: any) => {
      const received = `From Send: ${data}`;
      console.log(received);
      this.messages.push(received);
    });
  }

  selectSim(instanceId: string) {
    this._instanceId = instanceId;
    
    this.messages = [];
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

interface Notification{
  messageId: string;
  dateStamp: string;
  channel: EntityHeader;
  verbosity: EntityHeader;
  channelId: string;
  title: string;
  text: string;
  payloadType: string;
  payload: string;
}

interface Simulator {
  instanceName: string;
  simulatorName: string;
  instanceId: string;
  currentState: string;
  isActive: string;
  lastUpdate: string;
}
