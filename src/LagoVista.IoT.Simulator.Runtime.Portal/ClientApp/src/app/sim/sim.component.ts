import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sim',
  templateUrl: './sim.component.html',
  styleUrls: ['./sim.component.css']
})
export class SimComponent implements OnInit {
   public simulators: Simulator[];

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log(baseUrl);

      http.get<Simulator[]>(baseUrl + 'api/simnetwork/simulators').subscribe(result => {
        console.log(result);
        this.simulators = result;
      }, error => console.error(error));

  }

  ngOnInit() {
  }

}

interface Simulator {
  instanceName: string;
  simulatorName: string;
  instanceId: string;
  currentState: string;
  isActive: string;
}
