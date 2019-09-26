import { Component, OnInit, Input } from '@angular/core';
import { Production } from '../../production.model';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/shared/auth.service';
import { ProductionService } from '../../production.service';
import { ActivatedRoute, Params } from '@angular/router';
import { DaysService } from 'src/app/shared/days/days.service';

@Component({
  selector: 'app-production-find-show',
  templateUrl: './production-find-show.component.html',
  styleUrls: ['./production-find-show.component.css']
})
export class ProductionFindShowComponent implements OnInit {
  @Input() inputId: number;
  isFetching = false;
  isError = false;
  error: string = '';
  production: Production[] = [];
  singleProd: Production;
  id: string = '';
  job: string = '';
  subscriptions: Subscription[] = []
  total: number = 0;
  quantity: Production;
  editMode: boolean = false;
  editMulti: boolean[] = [];
  inQuestion = {
    inQuestion: "True"
  }

  constructor(
    private pro: ProductionService,
    private route: ActivatedRoute,
    private dayServ: DaysService
  ) { }

  ngOnInit() {
    if (this.inputId){
      this.getSingleProduction();
    } else {
      this.subscriptions.push(
        this.route.params.subscribe((params: Params) => {
          this.job = params['job'];
          this.getJobProduction();
        })
      )
    }
    this.subscriptions.push(
      this.pro.proChanged.subscribe(()=>{
        if (this.inputId){
          this.getSingleProduction();
          this.editMode = false;
        } else {
          this.editMulti = [];
          this.total = 0;
          this.getJobProduction();
        }
      })
    )
  }

  getJobProduction() {
    this.isFetching = true;
    this.pro.fetchProduction("job="+this.job)
      .subscribe(production => {
        this.production = production;
        this.dayServ.dates = [];
        this.production.forEach(lot => {
          this.editMulti.push(false);
          let beginning = lot.date.substring(6,10);
          lot.date = beginning + "-" + lot.date.substring(0,4);
          this.total = +lot.quantity + this.total;
        })
        this.isFetching = false;
      }, error => {
        this.isFetching = false;
        this.isError = true;
        this.error = error.message
      })
  }  

  getSingleProduction() {
    this.isFetching = true;
    this.pro.fetchProductionById(this.inputId)
      .subscribe(production => {
        this.singleProd = production;
        this.dayServ.dates = [];
        let beginning = this.singleProd.date.substring(6,10);
        this.singleProd.date = beginning + "-" + this.singleProd.date.substring(0,4);
        this.total = +this.singleProd.quantity + this.total;
        this.isFetching = false;
      }, error => {
        this.isFetching = false;
        this.isError = true;
        this.error = error.message
      })
  }  

  onEdit(){
    this.editMode = true;
  }

  onEditMulti(set){
    this.editMulti[set] = true;
  }

  lotInQuestion(id){
    this.inQuestion.inQuestion = "True"
    this.pro.setInQuestion(this.inQuestion, id).subscribe(()=>{
      this.pro.proChanged.next()
    })
  }

  lotIsGood(id){
    this.inQuestion.inQuestion = "False"
    this.pro.setInQuestion(this.inQuestion, id).subscribe(()=>{
      this.pro.proChanged.next()
    })
  }

}
