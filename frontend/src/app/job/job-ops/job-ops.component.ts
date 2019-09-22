import { Component, OnInit, Input } from '@angular/core';
import { Operation } from 'src/app/job/job-ops/operation.model';
import { OpService } from 'src/app/job/job-ops/operation.service';
import { DaysService } from 'src/app/shared/days/days.service';
import { AuthService } from '../../shared/auth.service';

@Component({
  selector: 'app-job-ops',
  templateUrl: './job-ops.component.html',
  styleUrls: ['./job-ops.component.css']
})
export class JobOpsComponent implements OnInit {
  @Input() operation: string;
  isFetching = false;
  isError = false;
  error = '';
  operations: Operation[] = [];
  id = '';
  showForm = false;

  constructor(
    private operationServ: OpService,
    private dayServ: DaysService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.getOps();
  }

  onDelete(operation: Operation){
    if (confirm(
      "Are you sure you want to delete " + operation.opNumber + " for job # " 
      + operation.jobNumber + "?"
      )){
        let searchForDelete = operation.opNumber + "&job=" + operation.jobNumber;
        this.operationServ.deleteOp(searchForDelete).subscribe(()=>{
        setTimeout(()=>{this.getOps()},)}
      );
      this.operationServ.opsChanged.next();
    }
  }

  getOps() {
    this.isFetching = true;
      this.operationServ.fetchOpByJob(this.operation)
        .subscribe(operation => {
          this.operations = operation;
          this.dayServ.dates = [];
          this.isFetching = false;
        }, error => {
          this.isFetching = false;
          this.isError = true;
          this.error = error.message
        })
  }

  newOp(){
    this.showForm = true;
  }

}
