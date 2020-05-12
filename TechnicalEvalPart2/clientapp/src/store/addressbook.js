import Vapi from 'vuex-rest-api'

export const addressbook = new Vapi({
  baseURL: '/api',
  state: {
    contacts: [],
    contact: null
  }
})
  .get({
    action: 'get',
    property: 'contact',
    path: ({ id }) => `/contacts/${id}`
  })
  .get({
    action: 'list',
    property: 'contacts',
    path: '/contacts',
    queryParams: true
  })
  .put({
    action: 'update',
    property: 'contact',
    path: ({ id }) => `/contacts/${id}`
  })
  .post({
    action: 'create',
    property: 'contact',
    path: '/contacts'
  })
  .delete({
    action: 'delete',
    property: 'contact',
    path: ({ id }) => `/contacts/${id}`
  })
  .getStore({
    namespaced: true
  })
