import { Component, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/shared/auth.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-machine',
  templateUrl: './machine.component.html',
  styleUrls: ['./machine.component.css']
})
export class MachineComponent implements OnDestroy{
  subscriptions: Subscription[] = [];


  constructor(
    public auth: AuthService,
    private route: ActivatedRoute
  ) {
    this.subscriptions.push(
      this.route.params.subscribe((params: Params) => {
        this.auth.machType = params["machType"];
      })
    ) 
  }

  ngOnDestroy(){
    this.subscriptions.forEach(sub=>{
      sub.unsubscribe();
    })
  }

}
