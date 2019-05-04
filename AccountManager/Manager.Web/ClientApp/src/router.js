import Vue from 'vue'
import Router from 'vue-router'
import Login from './components/Login.vue'
import Account from './components/Accounts.vue'
import store from './store'

Vue.use(Router)

const router =  new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'account',
      component: Account              
    },
    {
      path: '/login',
      name: 'login',
      component: Login,
      meta:{
        free:true
      }       
    }    
  ]
})

router.beforeEach((to, from, next) => {
// to and from are both route objects. must call `next`.
if (to.matched.some(record => record.meta.free)) {
  next()
}else if(store.state.user){
  next()
}else{
  next({
    name: 'login'
  })
}
})

export default router