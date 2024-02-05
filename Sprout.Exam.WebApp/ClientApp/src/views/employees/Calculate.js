import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
    this.state = { id: 0,fullName: '',birthdate: '',tin: '',typeId: 1,absentDays: 0,workedDays: 0,netIncome: 0, loading: true,loadingCalculate:false };
  }

  componentDidMount() {
    this.getEmployee(this.props.match.params.id);
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
  }

  handleSubmit(e){
      e.preventDefault();
      this.calculateSalary();
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-6'>
  <label>Full Name: </label>
  <input type='text' className='form-control' value={this.state.fullName}  readOnly/>
</div>

<div className='form-group col-md-6'>
  <label >Birthdate:</label>
  <input type='text' className='form-control' value={this.state.birthdate}  readOnly/>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-6'>
  <label>TIN:</label>
  <input type='text' className='form-control' value= {this.state.tin} readOnly/>
</div>

<div className='form-group col-md-6'>
  <label>Employee Type:</label>
  <input type='text' className='form-control' value={this.state.typeId === 1?"Regular": "Contractual"}  readOnly/>
</div>
</div>

<div className="form-row">

{ this.state.typeId === 1? 

  <div class="card col-6">
  <div class="card-header">
    Regular Employee
  </div>
  <div class="card-body">
    <div className='form-group col-md-12'><label>Salary: <b>20000</b> </label></div>
    <div className='form-group col-md-12'><label>Tax: <b>12%</b> </label></div>
    <div className='form-group'>
    <label htmlFor='inputAbsentDays4'>Absent Days: </label>
    <input type='text' className='form-control' id='inputAbsentDays4' onChange={this.handleChange.bind(this)} value={this.state.absentDays} name="absentDays" placeholder='Absent Days' />
  </div>
  </div>
</div>
 :
 <div className='card col-6'>
 <div class="card-header">
    Contractual Employee
 </div>
 <div class="card-body">
    <div className='form-group col-md-12'><label>Rate Per Day: <b>500</b> </label></div>
    <label htmlFor='inputWorkDays4'>Worked Days: </label>
    <input type='text' className='form-control' id='inputWorkDays4' onChange={this.handleChange.bind(this)} value={this.state.workedDays} name="workedDays" placeholder='Worked Days' />
 </div>
 
</div>

}

<div className='card col-6'>
  <div class="card-header">
      Net Income:
  </div>
  <div class="card-body">
    <input type='text' className='form-control font-weight-bold' value= {this.state.netIncome} readOnly/>
  </div>
</div>
</div>

<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-warning mr-2">{this.state.loadingCalculate?"Loading...": "Calculate"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-danger">Back</button>
    </form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Salary Calculator</h1>
        <br/>
        {contents}
      </div>
    );
  }

  async calculateSalary() {
    this.setState({ loadingCalculate: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'POST',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify({id: this.state.id,absentDays: this.state.absentDays,workedDays: this.state.workedDays})
    };
    const response = await fetch('api/employees/' + this.state.id + '/calculate',requestOptions);
    const data = await response.json();
    this.setState({ loadingCalculate: false,netIncome: data });
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if(response.status === 200){
        const data = await response.json();
        this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId, loading: false,loadingCalculate: false });
    }
    else{
        alert("There was an error occured.");
        this.setState({ loading: false,loadingCalculate: false });
    }
  }
}
