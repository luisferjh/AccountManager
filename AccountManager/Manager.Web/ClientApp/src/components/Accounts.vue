<template>   
    <div class="accountComponent">			
			<b-row>
				<!-- Reload all accounts-->
				<b-button class="btn-style-user mr-4" variant="outline-secondary"	
				@click="listar()"
				>
				<i class="fas fa-redo"></i>
				</b-button>	

				<!-- Add New account -->
				<b-button class="btn-style-user" variant="outline-primary"
					v-b-modal.modal-add-account 			
				>
					<i class="fas fa-plus"></i>
				</b-button>			
			</b-row>

			<!-- fila para las card de todas las cuentas-->
			<b-row class="justify-content-md-between">			 
				<b-card v-for="account in accounts" :key='account.idAccount'				
					v-bind:title='account.webAccountName'
					@click="showAccountData(account.idAccount)"
					v-b-modal.modal-1            						      
					tag="article"
					style="max-width: 20rem;"
					class="mb-2"							
        >
					<b-card-text>
						{{account.email}}
					</b-card-text>

				  <!--  boton editar -->			
					<!-- button for another way -->
 					<b-button id="edit-btn" @click.stop="showModalEdit(account.idAccount)" variant="success">
						<i class="far fa-edit"></i>
					</b-button> 

					<!--  button Remove -->
					<!-- button for another way -->
					<b-button id="remove-btn" @click.stop="showModalRemove(account.idAccount)" variant="danger">
						<i class="far fa-trash-alt"></i>
					</b-button>
        </b-card>

					<!-- modal para mostrar una nueva cuenta -->
					<b-modal id="modal-add-account" title="Adding account"
					:ok-only=true					
					:ok-title="'Exit'"					
					>
					<!-- Form to add new account -->
						<b-form @submit="onSubmit">
							<b-form-group id="input-group-1" label="Your Account Name:" label-for="input-1">
							<b-form-input
								id="input-1"
								v-model="nameWebAccount"
								required
								placeholder="Enter name account"
							></b-form-input>
							</b-form-group>
						
								<!--User -->
							<b-form-group id="input-group-2" label="User:" label-for="input-2">
								<b-form-input
									id="input-2"
									v-model="user"					
									placeholder="Enter user"
								></b-form-input>
							</b-form-group>

							<!-- password -->					
							<b-form-group id="input-group-3" label="Password:" label-for="input-3">
								<b-form-input
									id="input-3"
									v-model="password"							
									placeholder="Enter password"
								></b-form-input>
							</b-form-group>

							<!--Email -->
							<b-form-group
								id="input-group-4"
								label="Email address:"
								label-for="input-4"
								description="We'll never share your email with anyone else."
     					>
							<b-form-input
								id="input-4"		
							  v-model="email"						
								type="email"
								required
								placeholder="Enter email"
        			>
							</b-form-input>
							</b-form-group>

								<!-- Description -->
							<b-form-group id="input-group-4" label="Description:" label-for="textarea">
								<b-form-textarea
									id="textarea"
									v-model="description"								
									placeholder="Description"
								></b-form-textarea>
							</b-form-group>

							<b-button type="submit" variant="success">Submit</b-button>
							<!-- <b-button type="reset" variant="danger">Reset</b-button> -->
						</b-form>
					</b-modal>		
				
					<!-- modal para mostrar datos de cada cuenta -->		
          <b-modal id="modal-1" title="Resumen cuenta"
						:ok-only="true"
						:ok-title="'Exit'"						  
					>						
						<p class="my-4"><strong>Nombre Cuenta:</strong> {{accountsFull.webAccountName}}</p>
						<p class="my-4"><strong>Usuario:</strong> {{accountsFull.userAccount}}</p>
						<p class="my-4"><strong>Password:</strong> {{accountsFull.password}}</p>
						<p class="my-4"><strong>Email:</strong> {{accountsFull.email}}</p>
            <p class="my-4"><strong>Descripcion:</strong> {{accountsFull.description}}</p>
          </b-modal>		

					 <!-- Modal Component for edit account -->
					 <b-modal ref="edit-modal" title="Edit Account">
						 <!-- Form to Edit/Update Account -->
						<b-form @submit="onEdit()">
							<b-form-group id="input-group-1" label="Your Account Name:" label-for="input-1">
							<b-form-input
								id="input-1"								
								v-model="accountsFull.webAccountName"											
								required
								placeholder="Enter name account"
							></b-form-input>
							</b-form-group>
						
								<!--User -->
							<b-form-group id="input-group-2" label="User:" label-for="input-2">
								<b-form-input
									id="input-2"
									v-model="accountsFull.userAccount"					
									placeholder="Enter user"
								></b-form-input>
							</b-form-group>
							<!-- password -->					
							<b-form-group id="input-group-3" label="Password:" label-for="input-3">
								<b-form-input
									id="input-3"
									v-model="accountsFull.password"							
									placeholder="Enter password"
								></b-form-input>
							</b-form-group>

							<!--Email -->
							<b-form-group
								id="input-group-4"
								label="Email address:"
								label-for="input-4"
								description="We'll never share your email with anyone else."
     					>
							<b-form-input
								id="input-4"		
							  v-model="accountsFull.email"						
								type="email"
								required
								placeholder="Enter email"
        			>
							</b-form-input>
							</b-form-group>

								<!-- Description -->
							<b-form-group id="input-group-4" label="Description:" label-for="textarea">
								<b-form-textarea
									id="textarea"
									v-model="accountsFull.description"								
									placeholder="Description"
								></b-form-textarea>
							</b-form-group>

							<b-button @click="editAccount" variant="success">Update</b-button>
							<!-- <b-button type="reset" variant="danger">Reset</b-button> -->
						</b-form>
					</b-modal>
		
					<!-- modal to remove one account -->		
					<b-modal ref="remove-modal" title="Remove Account"
					 :ok-title="'Delete'"
					 @ok="deleteAccount(accountsFull.idAccount)"
					>			
						<p class="my-4"><strong>¿Are you sure to delete this account?</strong></p>
						<p class="my-4"><strong>Nombre Cuenta:</strong> {{accountsFull.webAccountName}}</p>
						<p class="my-4"><strong>Usuario:</strong> {{accountsFull.userAccount}}</p>
						<p class="my-4"><strong>Password:</strong> {{accountsFull.password}}</p>
						<p class="my-4"><strong>Email:</strong> {{accountsFull.email}}</p>
            <p class="my-4"><strong>Descripcion:</strong> {{accountsFull.description}}</p>
						 <!-- <p class="my-4"><strong>prueba:</strong> {{accounts}}</p> -->
					</b-modal>
			</b-row>	
		</div>
		
</template>

<script>

	import axios from 'axios'
  import { mapState } from 'vuex';
	
	export default {   
		// props:['searchString'],     
		data(){
			return {
				accounts:[],				
				accountsFull:[],  
				id: '',                     
				nameWebAccount:'',
				user:'',
				password:'', 
				description:'',                                                 
				email: '',   				
				editedIndex: -1                                                                               
			}
		},  
		computed : mapState(['searchString']),
		watch:{
			//esto se ejecuta por cada caracter ingresado
			//y ejecutara la busqueda y buscara las coincidencias
			//escritas en la barra de busqueda
			searchString(newValue){
				let me = this;				
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}
				console.log(`${newValue}`)
				//si esta la cadena de busqueda esta
				//vacia, no ejecuta la peticion
				if (newValue.length !== 0) {
					axios.get('/api/Accounts/search/'+ newValue,configuration)
						.then(function(response){								
							me.accounts = response.data													
						}).catch(function(error){
								console.log(`${error} no encontrado`);					
						});
				}
				//si esta vacio mostramos todas las cuentas
				else{
					this.listar();
				}		
			}

		},    
		created () {
			this.listar();
		},
		methods:{				
			//modal methods to open modal
			showModalEdit(id) {
				this.$refs['edit-modal'].show()
				this.showAccountData(id)
				console.log(this.searchString)
			},
			showModalRemove(id) {
				this.$refs['remove-modal'].show()
				this.showAccountData(id)
			},
			//HTTP Request methods 
			listar:function(){
				let me=this;
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}
				// axios.get('api/v2/pokemon/1/')
				axios.get('/api/accounts/MainData', configuration)							
					.then(function(response){
						console.log(response.data);
						me.accounts=response.data;					
					}).catch(function(error){
						console.log(error);
						console.log(`error 2 ${error.response}`);
						console.log(`error 3 ${error.response.status}`);						
						// if (error.response.status === 401) {						
						// 	me.$store.dispatch('exit')
    				// }
							
					});				
			},
			//show basc data accounts email and account
			showAccountData:function(id,e){					
				let me=this;						
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}											
				axios.get('/api/accounts/show/'+id, configuration)
				.then(function(response){								
					me.accountsFull = response.data										
					// console.log(me.accountsFull)					
				}).catch(function(error){
						console.log(error);
				});
			},
				//Post Accounts
			onSubmit:function(evt) {			
				let me = this;
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}									
				evt.preventDefault()					
				axios.post('/api/accounts/Create',{
						"WebAccountName":this.nameWebAccount,
						"UserAccount":this.user,
						"Password":this.password,
						"Description":this.description,
						"Email":this.email
				}, configuration).then(function(response){            												
						me.listar();
				}).catch(function(error){
						console.log(error);
				});		
				this.onReset()
					
			},
			//Update/Edit account
			editAccount(){	
				let me = this;	
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}																	
				axios.put('/api/accounts/update',{
					"IdAccount":this.accountsFull.idAccount,
					"WebAccountName":this.accountsFull.webAccountName,
					"UserAccount":this.accountsFull.userAccount,
					"Password":this.accountsFull.password,
					"Description":this.accountsFull.description,
					"Email":this.accountsFull.email
				}, configuration).then(function(response){            				
						me.listar();											
				}).catch(function(error){
					console.log(error);
				});	
				this.onReset()	
				this.$refs['edit-modal'].hide()		
			},
			deleteAccount(id){
				let me = this; //hacemos esto para no perder la referencias del this dentro de las funciones de axios
				let header = {"Authorization" : "Bearer " + this.$store.state.token}
				let configuration = {headers : header}									
				axios.delete('/api/accounts/delete/'+id,configuration)
				.then(function(response){													
					console.log(response)		
					me.listar();			
				}).catch(function(error){
						console.log(error);
				});
				this.$refs['remove-modal'].hide()
				console.log(this.accounts)
		
			},			    
			onReset:function(){				
				//Reset our form values
				this.nameWebAccount = ''
				this.user = ''
				this.password = ''
				this.description = ''
				this.email = ''
			}                          
		}        
	}
</script>

<style lang="sass" scoped>
@import "../styles/components/account.scss";
</style>


