import { Component, OnInit } from '@angular/core';
import { MachineService } from 'src/app/machine/machine.service';
import { OpService } from 'src/app/job/job-ops/operation.service';
import { JobService } from 'src/app/job/job.service';
import { Machine } from 'src/app/machine/machine.model';
import { MillSet } from './mill-set.model';
import { AuthService } from 'src/app/shared/auth.service';

@Component({
  selector: 'app-production-mill',
  templateUrl: './production-mill.component.html',
  styleUrls: ['./production-mill.component.css']
})
export class ProductionMillComponent implements OnInit {
  millSets: MillSet[][] = [];
  machines = [];
  editMode = false;

  constructor(
    private machServ: MachineService,
    private jobServ: JobService,
    private opServ: OpService,
    public auth: AuthService
  ) { }

  ngOnInit() {
    this.opServ.opsChanged.subscribe(()=>{
      this.editMode = false;
      this.millSets = [];
      this.getProduction();
    })
    this.getProduction();
  }

  getProduction(){
    this.machServ.fetchMachinesByType().subscribe(machines=>{
      this.machines = machines
      this.jobServ.fetchJobsByType().subscribe((jobs=>{
        machines.forEach(machine=>{
          if (machine.machine.includes(" ")){
            machine.machine = this.auth.splitJoin(machine.machine);
          }
          let millSetsHold: MillSet[] = []
          let used = 0;
          jobs.result.forEach(job=>{
            this.opServ.fetchOpByMachAndJob(machine.machine+"&job="+job.jobNumber)
            .subscribe(ops=>{
              if (ops.length > 0){
                let millSetHold: MillSet = {
                  machine: this.auth.rejoin(machine.machine),
                  jobNumber: job.jobNumber,
                  partNumber: job.partNumber,
                  ops: ops
                }
                millSetsHold.push(millSetHold)
              }
              used += 1;
              if (used == jobs.result.length){
                this.millSets.push(millSetsHold);
              }
            })
          })
        })
      }))
    });
  }
  //display part totals in each operation by machine they are running on, 
  // and remaining hours for op on the job, and on the monthly requirement  

  changeEdit(){
    this.editMode = !this.editMode;
  }

}
